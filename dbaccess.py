import mysql.connector
import base64
from Crypto.Cipher import XOR


connection_config = {
    'user': 'root',
    'password': 'oprisa',
    'host': '127.0.0.1',
    'database': 'eliza',
}


def executesql(sql, params=()):
    try:
        connection = mysql.connector.connect(**connection_config)
        cursor = connection.cursor()
        cursor.execute(sql, params)
        connection.commit()
        cursor.close()
        connection.close()
        return True
    except mysql.connector.errors.Error as err:
        print err
        return False


def querydb(sql, params=()):
    try:
        connection = mysql.connector.connect(**connection_config)
        cursor = connection.cursor()
        cursor.execute(sql, params)
        dbdata = cursor.fetchall()
        cursor.close()
        connection.close()
        if len(dbdata) == 0:
            return None
        return dbdata
    except mysql.connector.errors.Error as err:
        print err
        return None


def get_user_id(username):
    sql = "SELECT account_id FROM accounts WHERE user_name = %s"
    dbdata = querydb(sql, (username,))
    if len(dbdata) < 0:
        return -1
    return dbdata[0][0]


def create_user_account(username, password):
    sql = "INSERT INTO accounts (user_name, password, online_status) VALUES (%s, %s, 0)"
    cipher = XOR.new('random')
    password_encrypted = base64.b64encode(cipher.encrypt(password))
    return executesql(sql, (username, password_encrypted))


def user_login(username, password):
    sql = ("SELECT account_id FROM accounts "
           "WHERE user_name = %s AND password = %s")
    cipher = XOR.new('random')
    password_encrypted = base64.b64encode(cipher.encrypt(password))
    dbdata = querydb(sql, (username, password_encrypted))
    if dbdata is None:
        return -1
    return dbdata[0][0]


def update_user_online_status(userid, online_status):
    sql = "UPDATE accounts SET online_status = %s WHERE account_id = %s"
    return executesql(sql, (online_status, userid))


def create_friend_request(userid1, userid2):
    sql = "INSERT INTO friendrequests (user_from, user_to) VALUES (%s, %s)"
    return executesql(sql, (userid1, userid2))


def friend_request_sent(userid1, userid2):
    sql = "SELECT * FROM friendrequests WHERE user_from = %s AND user_to = %s"
    dbdata = querydb(sql, (userid1, userid2))
    return dbdata is not None


def get_friendship_status(userid1, userid2):
    sql = ("SELECT * FROM friendships WHERE (user1 = %s AND user2 = %s) OR "
           "(user1 = %s AND user2 = %s)")
    dbdata = querydb(sql, (userid1, userid2, userid2, userid1))
    return dbdata is not None


def accept_friend_request(userid1, userid2):
    sql1 = "DELETE FROM friendrequests WHERE user_from = %s AND user_to = %s"
    sql2 = "INSERT INTO friendships (user1, user2) VALUES (%s, %s)"
    if not executesql(sql1, (userid1, userid2)):
        return False
    return executesql(sql2, (userid1, userid2))


def delete_friendship(userid1, userid2):
    sql = ("DELETE FROM friendships WHERE (user1 = %s AND user2 = %s) OR "
           "(user1 = %s AND user2 = %s)")
    return executesql(sql, (userid1, userid2, userid2, userid1))


def add_block(userid1, userid2):
    sql = "INSERT INTO blocks (user1, user2) VALUES (%s, %s)"
    return executesql(sql, (userid1, userid2))


def remove_block(userid1, userid2):
    sql = "DELETE FROM blocks WHERE user1 = %s AND user2 = %s"
    return executesql(sql, (userid1, userid2))


def is_user_blocked(userid_blocking, userid_blocked):
    sql = "SELECT * FROM blocks WHERE user1 = %s AND user2 = %s"
    dbdata = querydb(sql, (userid_blocking, userid_blocked))
    return dbdata is not None


def get_friends(userid):
    sql = ("SELECT a.user_name, a.online_status FROM accounts a "
           "JOIN friendships f ON (a.account_id = f.user2) "
           "WHERE f.user1 = %s "
           "UNION "
           "SELECT a.user_name, a.online_status FROM accounts a "
           "JOIN friendships f ON (a.account_id = f.user1) "
           "WHERE f.user2 = %s")
    return querydb(sql, (userid, userid))


def change_password(userid, new_pass):
    sql = "UPDATE accounts SET password = %s WHERE account_id = %s"
    cipher = XOR.new('random')
    new_pass_encrypted = base64.b64encode(cipher.encrypt(new_pass))
    return executesql(sql, (new_pass_encrypted, userid))


def get_description(userid):
    sql = "SELECT description FROM accounts WHERE account_id = %s"
    return querydb(sql, (userid,))


def set_description(userid, description):
    sql = "UPDATE accounts SET description = %s WHERE account_id = %s"
    return executesql(sql, (description, userid))


def get_profile_picture(userid):
    sql = "SELECT profile_pic FROM accounts WHERE account_id = %s"
    return querydb(sql, (userid,))


def set_profile_picture(userid, profile_picture):
    sql = "UPDATE accounts SET profile_pic = %s WHERE account_id = %s"
    return executesql(sql, (profile_picture, userid))


def set_all_users_offline():
    sql = "UPDATE accounts SET online_status = 0"
    return executesql(sql)

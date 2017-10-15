import mysql.connector
import base64
from Crypto.Cipher import XOR


connection_config = {
    'user': 'root',
    'password': 'oprisa',
    'host': '127.0.0.1',
    'database': 'eliza',
}


def user_exists(username):
    try:
        connection = mysql.connector.connect(**connection_config)
        cursor = connection.cursor()
        sql = "SELECT account_id FROM accounts WHERE user_name = '{}'".format(username)
        cursor.execute(sql)
        dbdata = cursor.fetchall()
        cursor.close()
        connection.close()
        if len(dbdata) > 0:
            return dbdata[0][0]
        else:
            return -1
    except mysql.connector.errors.Error as err:
        print err
        return -1


def create_user_account(username, password):
    try:
        cipher = XOR.new('random')
        data = (username, base64.b64encode(cipher.encrypt(password)))
        connection = mysql.connector.connect(**connection_config)
        cursor = connection.cursor()
        sql = ("INSERT INTO accounts "
               "(user_name, password, online_status) "
               "VALUES (%s, %s, 0)")
        cursor.execute(sql, data)
        connection.commit()
        cursor.close()
        connection.close()
        return True
    except mysql.connector.errors.Error as err:
        print err
        return False


def user_login(username, password):
    try:
        cipher = XOR.new('random')
        data = (username, base64.b64encode(cipher.encrypt(password)))
        connection = mysql.connector.connect(**connection_config)
        cursor = connection.cursor()
        sql = ("SELECT account_id, profile_pic, description FROM accounts "
               "WHERE user_name = %s AND password = %s")
        cursor.execute(sql, data)
        dbdata = cursor.fetchall()
        cursor.close()
        connection.close()
        if len(dbdata) == 0:
            return None
        else:
            return dbdata[0]
    except mysql.connector.errors.Error as err:
        print err
        return None


def update_user_online_status(userid, online_status):
    try:
        connection = mysql.connector.connect(**connection_config)
        cursor = connection.cursor()
        sql = ("UPDATE accounts SET online_status = %s "
               "WHERE account_id = %s")
        cursor.execute(sql, (online_status, userid))
        connection.commit()
        cursor.close()
        connection.close()
    except mysql.connector.errors.Error as err:
        print err


def create_friend_request(userid1, userid2):
    try:
        connection = mysql.connector.connect(**connection_config)
        cursor = connection.cursor()
        sql = ("INSERT INTO friendrequests "
               "(user_from, user_to) "
               "VALUES (%s, %s)")
        cursor.execute(sql, (userid1, userid2))
        connection.commit()
        cursor.close()
        connection.close()
        return True
    except mysql.connector.errors.Error as err:
        print err
        return False


def friend_request_sent(userid1, userid2):
    try:
        connection = mysql.connector.connect(**connection_config)
        cursor = connection.cursor()
        sql = ("SELECT * FROM friendrequests "
               "WHERE user_from = %s AND user_to = %s")
        cursor.execute(sql, (userid1, userid2))
        dbdata = cursor.fetchall()
        cursor.close()
        connection.close()
        if len(dbdata) == 0:
            return False
        else:
            return True
    except mysql.connector.errors.Error as err:
        print err
        return False


def get_friendship_status(userid1, userid2):
    try:
        connection = mysql.connector.connect(**connection_config)
        cursor = connection.cursor()
        sql = ("SELECT * FROM friendships "
               "WHERE (user1 = %s AND user2 = %s) OR "
               "(user1 = %s AND user2 = %s)")
        cursor.execute(sql, (userid1, userid2, userid2, userid1))
        dbdata = cursor.fetchall()
        cursor.close()
        connection.close()
        if len(dbdata) == 0:
            return False
        else:
            return True
    except mysql.connector.errors.Error as err:
        print err
        return False


def accept_friend_request(userid1, userid2):
    try:
        connection = mysql.connector.connect(**connection_config)
        cursor = connection.cursor()
        sql = ("DELETE FROM friendrequests WHERE "
               "user_from = %s AND user_to = %s")
        cursor.execute(sql, (userid1, userid2))
        connection.commit()
        sql = ("INSERT INTO friendships (user1, user2) "
               "VALUES (%s, %s)")
        cursor.execute(sql, (userid1, userid2))
        connection.commit()
        cursor.close()
        connection.close()
        return True
    except mysql.connector.errors.Error as err:
        print err
        return False


def delete_friendship(userid1, userid2):
    try:
        connection = mysql.connector.connect(**connection_config)
        cursor = connection.cursor()
        sql = ("DELETE FROM friendships WHERE "
               "(user1 = %s AND user2 = %s) OR "
               "(user1 = %s AND user2 = %s)")
        cursor.execute(sql, (userid1, userid2, userid2, userid1))
        connection.commit()
        cursor.close()
        connection.close()
        return True
    except mysql.connector.errors.Error as err:
        print err
        return False


def add_block(userid1, userid2):
    try:
        connection = mysql.connector.connect(**connection_config)
        cursor = connection.cursor()
        sql = ("INSERT INTO blocks (user1, user2) "
               "VALUES (%s, %s)")
        cursor.execute(sql, (userid1, userid2))
        connection.commit()
        cursor.close()
        connection.close()
        return True
    except mysql.connector.errors.Error as err:
        print err
        return False


def remove_block(userid1, userid2):
    try:
        connection = mysql.connector.connect(**connection_config)
        cursor = connection.cursor()
        sql = ("DELETE FROM blocks WHERE "
               "user1 = %s AND user2 = %s")
        cursor.execute(sql, (userid1, userid2))
        connection.commit()
        cursor.close()
        connection.close()
        return True
    except mysql.connector.errors.Error as err:
        print err
        return False


def is_user_blocked(userid_blocking, userid_blocked):
    try:
        connection = mysql.connector.connect(**connection_config)
        cursor = connection.cursor()
        sql = ("SELECT * FROM blocks WHERE "
               "user1 = %s AND user2 = %s")
        cursor.execute(sql, (userid_blocking, userid_blocked))
        dbdata = cursor.fetchall()
        cursor.close()
        connection.close()
        if len(dbdata) == 0:
            return False
        else:
            return True
    except mysql.connector.errors.Error as err:
        print err
        return False

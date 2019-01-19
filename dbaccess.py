import mysql.connector
import base64
from Crypto.Cipher import XOR


connection_config = {
    'user': 'root',
    'password': 'root',
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
    if dbdata is None:
        return -1
    return dbdata[0][0]


def create_user_account(username, password):
    sql = "INSERT INTO accounts (user_name, password, online_status) VALUES (%s, %s, 0)"
    cipher = XOR.new('random')
    password_encrypted = base64.b64encode(cipher.encrypt(password))
    return executesql(sql, (username, password_encrypted))


def insert_message(timestamp, user1, user2, content):
    sql = "INSERT INTO messages (timestamp, user1, user2, content) VALUES (%s, %s, %s, %s)"
    return executesql(sql, (timestamp, user1, user2, content))


def get_messages(user1, user2):
    sql = ("SELECT m.timestamp, a.user_name, m.content FROM messages m "
           "JOIN accounts a ON (a.account_id = m.user1) "
           "WHERE (m.user1 = %s AND m.user2 = %s) OR (m.user1 = %s AND m.user2 = %s) "
           "ORDER BY m.timestamp")
    return querydb(sql, (user1, user2, user2, user1))


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


def decline_friend_request(userid1, userid2):
    sql = "DELETE FROM friendrequests WHERE user_from = %s AND user_to = %s"
    return executesql(sql, (userid1, userid2))


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


def get_friend_requests(userid):
    sql = ("SELECT a.user_name FROM accounts a "
           "JOIN friendrequests f ON (a.account_id = f.user_from) "
           "WHERE f.user_to = %s")
    return querydb(sql, (userid, ))


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


def get_song_data(song_name):
    sql = "SELECT mp3_data FROM songs WHERE song_name = %s"
    return querydb(sql, (song_name, ))


def get_room_id(room_name):
    sql = "SELECT id FROM chatrooms WHERE name = %s"
    dbdata = querydb(sql, (room_name,))
    if dbdata is None:
        return -1
    return dbdata[0][0]


def createroom(userid, room_name, public):
    sql = "INSERT INTO chatrooms (name, owner, public) VALUES (%s, %s, %s)"
    executesql(sql, (room_name, userid, public))
    return addtoroom(userid, get_room_id(room_name))


def get_room_owner(room_name):
    sql = "SELECT owner FROM chatrooms WHERE name = %s"
    dbdata = querydb(sql, (room_name,))
    if dbdata is None:
        return -1
    return dbdata[0][0]


def deleteroom(room_name):
    roomid = get_room_id(room_name)
    sql = "DELETE FROM chatroom_messages WHERE chatroom = %s"
    executesql(sql, (roomid,))
    sql = "DELETE FROM chatroom_memberships WHERE chatroom = %s"
    executesql(sql, (roomid,))
    sql = "DELETE FROM chatrooms WHERE name = %s"
    return executesql(sql, (room_name,))


def is_room_private(room_name):
    sql = "SELECT public FROM chatrooms WHERE name = %s"
    dbdata = querydb(sql, (room_name,))
    if dbdata is None:
        return True
    if dbdata[0][0] == 1:
        return False
    return True


def ismember(userid, roomid):
    sql = "SELECT * FROM chatroom_memberships WHERE user = %s AND chatroom = %s"
    dbdata = querydb(sql, (userid, roomid))
    if dbdata is None:
        return False
    else:
        return True


def addtoroom(memberid, roomid):
    sql = "INSERT INTO chatroom_memberships (user, chatroom) VALUES (%s, %s)"
    return executesql(sql, (memberid, roomid))


def kickfromroom(memberid, roomid):
    sql = "DELETE FROM chatroom_memberships WHERE user = %s AND chatroom = %s"
    return executesql(sql, (memberid, roomid))


def getrooms(userid):
    sql = ("SELECT DISTINCT chatrooms.name FROM chatrooms JOIN chatroom_memberships ON "
           "(chatrooms.id = chatroom_memberships.chatroom) WHERE "
           "chatroom_memberships.user = %s OR chatrooms.public = 1")
    return querydb(sql, (userid,))


def get_room_messages(roomid):
    sql = ("SELECT m.timestamp, a.user_name, m.content FROM chatroom_messages m "
           "JOIN accounts a ON (a.account_id = m.user) "
           "WHERE m.chatroom = %s "
           "ORDER BY m.timestamp")
    return querydb(sql, (roomid,))


def insert_room_message(timestamp, userid, roomid, content):
    sql = "INSERT INTO chatroom_messages (timestamp, user, chatroom, content) VALUES (%s, %s, %s, %s)"
    return executesql(sql, (timestamp, userid, roomid, content))


def get_member_names(roomid):
    sql = ("SELECT a.user_name FROM accounts a JOIN chatroom_memberships m ON "
           "(a.account_id = m.user) JOIN chatrooms c ON (m.chatroom = c.id) "
           "WHERE c.id = %s")
    return querydb(sql, (roomid,))

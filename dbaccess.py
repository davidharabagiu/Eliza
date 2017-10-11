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
            return True
        else:
            return False
    except mysql.connector.errors.Error as err:
        print err
        return False


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

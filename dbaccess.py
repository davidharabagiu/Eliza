import mysql.connector
import base64
from Crypto.Cipher import XOR


connection_config = {
    'user': 'root',
    'password': 'oprisa',
    'host': '127.0.0.1',
    'database': 'eliza',
}


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
    except mysql.connector.errors.Error as err:
        print 'MySQL error: {}'.format(err)
        exit(1)


def user_login(username, password):
    try:
        cipher = XOR.new('random')
        data = (username, base64.b64encode(cipher.encrypt(password)))
        connection = mysql.connector.connect(**connection_config)
        cursor = connection.cursor()
        sql = ("SELECT profile_pic, description FROM accounts "
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
        print 'MySQL error: {}'.format(err)
        exit(1)

import mysql.connector

connection_config = {
    'user': 'root',
    'password': 'oprisa',
    'host': '127.0.0.1',
    'database': 'eliza',
}

if __name__ == '__main__':
    connection = mysql.connector.connect(**connection_config)
    print 'Connection to database established successfully'
    connection.close()

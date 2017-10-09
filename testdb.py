import dbaccess

"""
username = raw_input('user: ')
password = raw_input('password: ')
dbaccess.create_user_account(username, password)
"""


username = raw_input('user: ')
password = raw_input('password: ')
userdata = dbaccess.user_login(username, password)
if userdata is None:
    print 'Login failed'
else:
    print 'Success'

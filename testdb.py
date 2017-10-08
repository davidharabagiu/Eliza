import dbaccess


username = raw_input('user: ')
password = raw_input('password: ')
if dbaccess.create_user_account(username, password):
    print 'Success'
else:
    print 'User already exists'

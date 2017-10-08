import dbaccess


username = raw_input('user:')
password = raw_input('password:')

userdata = dbaccess.user_login(username, password)
if userdata is None:
    print 'Login failed'
else:
    print 'Logged in as {}'.format(username)

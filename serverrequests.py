import dbaccess


def register(username, password):
    if len(username) < 5:
        return 'The username must be at least 5 characters long'
    elif len(username) < 5:
        return 'The password must be at least 5 characters long'
    elif len(username) > 20:
        return 'The username must be at most 20 characters long'
    elif len(password) > 30:
        return 'The password must be at most 40 characters long'
    elif not username.isalnum():
        return 'The username must only contain letters and digits'
    elif dbaccess.user_exists(username):
        return 'This user name already exists'
    elif not dbaccess.create_user_account(username, password):
        return 'Database error'
    else:
        return 'User account was created successfully'


def login(username, password, clients_logged_in, client):
    if username in clients_logged_in.keys():
        return 'User already logged in'
    else:
        userdata = dbaccess.user_login(username, password)
        if userdata is None:
            return 'Invalid credentials'
        else:
            clients_logged_in[username] = (client, userdata)
            dbaccess.update_user_online_status(userdata[0], 1)
            return 'Login successful'


def logout(username, clients_logged_in):
    if username not in clients_logged_in.keys():
        return 'User not logged in'
    else:
        dbaccess.update_user_online_status(clients_logged_in[username][1][0], 0)
        del clients_logged_in[username]
        return 'Logout successful'


def sendmsg(userfrom, message, userto, clients_logged_in):
    if userfrom not in clients_logged_in.keys():
        return 'Not logged in'
    elif userto not in clients_logged_in.keys():
        return 'User not online'
    elif len(message) < 1:
        return 'Message can\'t be empty'
    else:
        clients_logged_in[userto][0].sendall('msg from ' + userfrom + ': ' + message)
        return 'Message sent successfully'


def queryonline(username, clients_logged_in):
    if username in clients_logged_in.keys():
        return 'Yes'
    else:
        return 'No'

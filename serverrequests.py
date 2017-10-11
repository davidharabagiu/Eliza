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

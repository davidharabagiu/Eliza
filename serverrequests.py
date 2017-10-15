import dbaccess
import utils


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
    elif dbaccess.user_exists(username) >= 0:
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
        clients_logged_in[userto][0].sendall('msg from ' + userfrom + ': ' + utils.concatlist(message))
        return 'Message sent successfully'


def queryonline(username, clients_logged_in):
    if username in clients_logged_in.keys():
        return 'Yes'
    else:
        return 'No'


def friendrequest(username_from, username_to, clients_logged_in):
    if username_from not in clients_logged_in.keys():
        return 'Not logged in'
    else:
        userid_from = dbaccess.user_exists(username_from)
        userid_to = dbaccess.user_exists(username_to)
        if userid_from >= 0 and userid_to >= 0:
            if dbaccess.friend_request_sent(userid_from, userid_to):
                return 'Friend request already sent'
            elif dbaccess.friend_request_sent(userid_to, userid_from):
                return 'You already got a friend request from this user'
            elif dbaccess.get_friendship_status(userid_from, userid_to):
                return 'This user is already your friend'
            elif dbaccess.create_friend_request(userid_from, userid_to):
                return 'Friend request sent successfully'
            else:
                return 'Database error'
        else:
            return 'The user does not exist'


def acceptfriendrequest(username_from, username_to, clients_logged_in):
    if username_to not in clients_logged_in.keys():
        return 'Not logged in'
    else:
        userid_from = dbaccess.user_exists(username_from)
        userid_to = dbaccess.user_exists(username_to)
        if userid_from >= 0 and userid_to >= 0:
            if not dbaccess.friend_request_sent(userid_from, userid_to):
                return 'There is no friend request from this user'
            elif dbaccess.get_friendship_status(userid_from, userid_to):
                return 'This user is already your friend'
            elif dbaccess.accept_friend_request(userid_from, userid_to):
                return 'Friend request accepted successfully'
            else:
                return 'Database error'
        else:
            return 'The user does not exist'


def queryfriendship(username1, username2, clients_logged_in):
    if username1 not in clients_logged_in.keys():
        return 'Not logged in'
    else:
        userid1 = dbaccess.user_exists(username1)
        userid2 = dbaccess.user_exists(username2)
        if userid1 >= 0 and userid2 >= 0:
            if dbaccess.get_friendship_status(userid1, userid2):
                return 'Yes'
            else:
                return 'No'
        else:
            return 'The user does not exist'


def queryfriendrequestsent(username1, username2, clients_logged_in):
    if username1 not in clients_logged_in.keys():
        return 'Not logged in'
    else:
        userid1 = dbaccess.user_exists(username1)
        userid2 = dbaccess.user_exists(username2)
        if userid1 >= 0 and userid2 >= 0:
            if dbaccess.friend_request_sent(userid1, userid2):
                return 'Yes'
            else:
                return 'No'
        else:
            return 'The user does not exist'


def queryfriendrequestreceived(username1, username2, clients_logged_in):
    if username1 not in clients_logged_in.keys():
        return 'Not logged in'
    else:
        userid1 = dbaccess.user_exists(username1)
        userid2 = dbaccess.user_exists(username2)
        if userid1 >= 0 and userid2 >= 0:
            if dbaccess.friend_request_sent(userid2, userid1):
                return 'Yes'
            else:
                return 'No'
        else:
            return 'The user does not exist'


def unfriend(username1, username2, clients_logged_in):
    if username1 not in clients_logged_in.keys():
        return 'Not logged in'
    else:
        userid1 = dbaccess.user_exists(username1)
        userid2 = dbaccess.user_exists(username2)
        if userid1 >= 0 and userid2 >= 0:
            if dbaccess.get_friendship_status(userid1, userid2):
                if dbaccess.delete_friendship(userid1, userid2):
                    return 'You are no longer friends'
                else:
                    return 'Database error'
            else:
                return 'You are not a friend of this user'
        else:
            return 'This user does not exist'


def block(username1, username2, clients_logged_in):
    if username1 not in clients_logged_in.keys():
        return 'Not logged in'
    else:
        userid1 = dbaccess.user_exists(username1)
        userid2 = dbaccess.user_exists(username2)
        if userid1 >= 0 and userid2 >= 0:
            if dbaccess.is_user_blocked(userid1, userid2):
                return 'You already blocked this user'
            elif dbaccess.add_block(userid1, userid2):
                return 'User blocked successfully'
            else:
                return 'Database error'
        else:
            return 'This user does not exist'


def unblock(username1, username2, clients_logged_in):
    if username1 not in clients_logged_in.keys():
        return 'Not logged in'
    else:
        userid1 = dbaccess.user_exists(username1)
        userid2 = dbaccess.user_exists(username2)
        if userid1 >= 0 and userid2 >= 0:
            if not dbaccess.is_user_blocked(userid1, userid2):
                return 'You haven\' blocked this user'
            elif dbaccess.remove_block(userid1, userid2):
                return 'User unblocked successfully'
            else:
                return 'Database error'
        else:
            return 'This user does not exist'


def queryblock(username1, username2, clients_logged_in):
    if username1 not in clients_logged_in.keys():
        return 'Not logged in'
    else:
        userid1 = dbaccess.user_exists(username1)
        userid2 = dbaccess.user_exists(username2)
        if userid1 >= 0 and userid2 >= 0:
            if not dbaccess.is_user_blocked(userid1, userid2):
                return 'Yes'
            else:
                return 'No'
        else:
            return 'This user does not exist'

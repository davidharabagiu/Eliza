import dbaccess
import utils
import requeststatus


def register(username, password):
    if len(username) < 5:
        return requeststatus.STATUS_USERNAME_TOO_SHORT
    elif len(password) < 5:
        return requeststatus.STATUS_PASSWORD_TOO_SHORT
    elif len(username) > 20:
        return requeststatus.STATUS_USERNAME_TOO_LONG
    elif len(password) > 30:
        return requeststatus.STATUS_PASSWORD_TOO_LONG
    elif not username.isalnum():
        return requeststatus.STATUS_USERNAME_NON_ALPHANUMERIC
    elif dbaccess.get_user_id(username) >= 0:
        return requeststatus.STATUS_USERNAME_ALREADY_EXISTS
    elif not dbaccess.create_user_account(username, password):
        return requeststatus.STATUS_DATABASE_ERROR
    else:
        return requeststatus.STATUS_SUCCESS


def login(username, password, clients_logged_in, client):
    if username in clients_logged_in.keys():
        return 'User already logged in'
    else:
        userid = dbaccess.user_login(username, password)
        if userid == -1:
            return 'Invalid credentials'
        else:
            clients_logged_in[username] = (client, userid)
            dbaccess.update_user_online_status(userid, 1)
            return 'Login successful'


def logout(username, clients_logged_in):
    if username not in clients_logged_in.keys():
        return 'User not logged in'
    else:
        dbaccess.update_user_online_status(clients_logged_in[username][1], 0)
        del clients_logged_in[username]
        return 'Logout successful'


def sendmsg(userfrom, message, userto, clients_logged_in):
    if userfrom not in clients_logged_in.keys():
        return 'Not logged in'
    elif userto not in clients_logged_in.keys():
        return 'User not online'
    elif len(message) < 1:
        return 'Message can\'t be empty'
    elif dbaccess.is_user_blocked(clients_logged_in[userto][1], clients_logged_in[userfrom][1]):
        return 'This user blocked you'
    elif dbaccess.is_user_blocked(clients_logged_in[userfrom][1], clients_logged_in[userto][1]):
        return 'You have blocked this user'
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
        userid_from = dbaccess.get_user_id(username_from)
        userid_to = dbaccess.get_user_id(username_to)
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
        userid_from = dbaccess.get_user_id(username_from)
        userid_to = dbaccess.get_user_id(username_to)
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
        userid1 = dbaccess.get_user_id(username1)
        userid2 = dbaccess.get_user_id(username2)
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
        userid1 = dbaccess.get_user_id(username1)
        userid2 = dbaccess.get_user_id(username2)
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
        userid1 = dbaccess.get_user_id(username1)
        userid2 = dbaccess.get_user_id(username2)
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
        userid1 = dbaccess.get_user_id(username1)
        userid2 = dbaccess.get_user_id(username2)
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


def blockuser(username1, username2, clients_logged_in):
    if username1 not in clients_logged_in.keys():
        return 'Not logged in'
    else:
        userid1 = dbaccess.get_user_id(username1)
        userid2 = dbaccess.get_user_id(username2)
        if userid1 >= 0 and userid2 >= 0:
            if dbaccess.is_user_blocked(userid1, userid2):
                return 'You already blocked this user'
            elif dbaccess.add_block(userid1, userid2):
                return 'User blocked successfully'
            else:
                return 'Database error'
        else:
            return 'This user does not exist'


def unblockuser(username1, username2, clients_logged_in):
    if username1 not in clients_logged_in.keys():
        return 'Not logged in'
    else:
        userid1 = dbaccess.get_user_id(username1)
        userid2 = dbaccess.get_user_id(username2)
        if userid1 >= 0 and userid2 >= 0:
            if not dbaccess.is_user_blocked(userid1, userid2):
                return 'You haven\'t blocked this user'
            elif dbaccess.remove_block(userid1, userid2):
                return 'User unblocked successfully'
            else:
                return 'Database error'
        else:
            return 'This user does not exist'


def queryblock(username_caller, username1, username2, clients_logged_in):
    if username_caller not in clients_logged_in.keys():
        return 'Not logged in'
    else:
        userid1 = dbaccess.get_user_id(username1)
        userid2 = dbaccess.get_user_id(username2)
        if userid1 >= 0 and userid2 >= 0:
            if dbaccess.is_user_blocked(userid1, userid2):
                return 'Yes'
            else:
                return 'No'
        else:
            return 'This user does not exist'


def queryfriends(username, clients_logged_in):
    if username not in clients_logged_in.keys():
        return 'Not logged in'
    else:
        userid = dbaccess.get_user_id(username)
        if userid >= 0:
            friendlist = dbaccess.get_friends(userid)
            if friendlist is None:
                return 'Database error'
            else:
                return 'Query friends list successful\n' + str(friendlist)
        else:
            return 'This user does not exist'


def changepassword(username, old_pass, new_pass, clients_logged_in):
    if username not in clients_logged_in.keys():
        return 'Not logged in'
    elif dbaccess.user_login(username, old_pass) is None:
        return 'Old password incorrect'
    elif dbaccess.change_password(dbaccess.get_user_id(username), new_pass):
        return 'Password changed successfully'
    else:
        return 'Database error'


def querydescription(username_caller, username, clients_logged_in):
    if username_caller not in clients_logged_in.keys():
        return 'Not logged in'
    else:
        userid = dbaccess.get_user_id(username)
        if userid >= 0:
            description = dbaccess.get_description(userid)
            if description is None:
                return 'Database error'
            return 'Query description successful\n' + str(description[0][0])
        else:
            return 'This user does not exist'


def setdescription(username, description, clients_logged_in):
    if username not in clients_logged_in.keys():
        return 'Not logged in'
    else:
        userid = dbaccess.get_user_id(username)
        if userid >= 0:
            if not dbaccess.set_description(userid, utils.concatlist(description)):
                return 'Database error'
            return 'Description set successfully'
        else:
            return 'This user does not exist'


def queryprofilepic(username_caller, username, clients_logged_in):
    if username_caller not in clients_logged_in.keys():
        return 'Not logged in'
    else:
        userid = dbaccess.get_user_id(username)
        if userid >= 0:
            description = dbaccess.get_profile_picture(userid)
            if description is None:
                return 'Database error'
            return 'Query profile picture successful\n' + str(description[0][0])
        else:
            return 'This user does not exist'


def setprofilepic(username, profile_pic, clients_logged_in):
    if username not in clients_logged_in.keys():
        return 'Not logged in'
    else:
        userid = dbaccess.get_user_id(username)
        if userid >= 0:
            if not dbaccess.set_profile_picture(userid, profile_pic):
                return 'Database error'
            return 'Profile picture set successfully'
        else:
            return 'This user does not exist'

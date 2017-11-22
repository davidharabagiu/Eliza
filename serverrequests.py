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


def queryuserexists(username):
    if dbaccess.get_user_id(username) == -1:
        return requeststatus.QUERYRESPONSE_FALSE
    else:
        return requeststatus.QUERYRESPONSE_TRUE


def login(username, password, clients_logged_in, client):
    if username in clients_logged_in.keys():
        return requeststatus.STATUS_ALREADY_LOGGED_IN
    else:
        userid = dbaccess.user_login(username, password)
        if userid == -1:
            return requeststatus.STATUS_INVALID_CREDENTIALS
        else:
            clients_logged_in[username] = (client, userid)
            dbaccess.update_user_online_status(userid, 1)
            return requeststatus.STATUS_SUCCESS


def logout(username, clients_logged_in):
    if username not in clients_logged_in.keys():
        return requeststatus.STATUS_NOT_LOGGED_IN
    else:
        dbaccess.update_user_online_status(clients_logged_in[username][1], 0)
        del clients_logged_in[username]
        return requeststatus.STATUS_SUCCESS


def sendmsg(userfrom, message, userto, clients_logged_in):
    if userfrom not in clients_logged_in.keys():
        return requeststatus.STATUS_NOT_LOGGED_IN
    elif userto not in clients_logged_in.keys():
        return requeststatus.STATUS_USER_NOT_ONLINE
    elif len(message) < 1:
        return requeststatus.STATUS_ERROR_EMPTY_MESSAGE
    elif dbaccess.is_user_blocked(clients_logged_in[userto][1], clients_logged_in[userfrom][1]):
        return requeststatus.STATUS_SENDER_BLOCKED
    elif dbaccess.is_user_blocked(clients_logged_in[userfrom][1], clients_logged_in[userto][1]):
        return requeststatus.STATUS_RECEIVER_BLOCKED
    else:
        clients_logged_in[userto][0].sendall('msg from ' + userfrom + ': ' + utils.concatlist(message, ' '))
        return requeststatus.STATUS_SUCCESS


def queryonline(username_caller, username, clients_logged_in):
    if username_caller not in clients_logged_in.keys():
        return requeststatus.STATUS_NOT_LOGGED_IN
    if username in clients_logged_in.keys():
        return requeststatus.QUERYRESPONSE_TRUE
    else:
        return requeststatus.QUERYRESPONSE_FALSE


def friendrequest(username_from, username_to, clients_logged_in):
    if username_from not in clients_logged_in.keys():
        return requeststatus.STATUS_NOT_LOGGED_IN
    else:
        userid_from = dbaccess.get_user_id(username_from)
        userid_to = dbaccess.get_user_id(username_to)
        if userid_from >= 0 and userid_to >= 0:
            if dbaccess.friend_request_sent(userid_from, userid_to):
                return requeststatus.STATUS_FRIEND_REQUEST_ALREADY_SENT
            elif dbaccess.friend_request_sent(userid_to, userid_from):
                return requeststatus.STATUS_FRIEND_REQUEST_ALREADY_RECEIVED
            elif dbaccess.get_friendship_status(userid_from, userid_to):
                return requeststatus.STATUS_ALREADY_FRIENDS
            elif dbaccess.create_friend_request(userid_from, userid_to):
                return requeststatus.STATUS_SUCCESS
            else:
                return requeststatus.STATUS_DATABASE_ERROR
        else:
            return requeststatus.STATUS_NON_EXISTENT_USER


def acceptfriendrequest(username_from, username_to, clients_logged_in):
    if username_to not in clients_logged_in.keys():
        return requeststatus.STATUS_NOT_LOGGED_IN
    else:
        userid_from = dbaccess.get_user_id(username_from)
        userid_to = dbaccess.get_user_id(username_to)
        if userid_from >= 0 and userid_to >= 0:
            if not dbaccess.friend_request_sent(userid_from, userid_to):
                return requeststatus.STATUS_NO_FRIEND_REQUEST
            elif dbaccess.get_friendship_status(userid_from, userid_to):
                return requeststatus.STATUS_ALREADY_FRIENDS
            elif dbaccess.accept_friend_request(userid_from, userid_to):
                return requeststatus.STATUS_SUCCESS
            else:
                return requeststatus.STATUS_DATABASE_ERROR
        else:
            return requeststatus.STATUS_NON_EXISTENT_USER


def declinefriendrequest(username_from, username_to, clients_logged_in):
    if username_to not in clients_logged_in.keys():
        return requeststatus.STATUS_NOT_LOGGED_IN
    else:
        userid_from = dbaccess.get_user_id(username_from)
        userid_to = dbaccess.get_user_id(username_to)
        if userid_from >= 0 and userid_to >= 0:
            if not dbaccess.friend_request_sent(userid_from, userid_to):
                return requeststatus.STATUS_NO_FRIEND_REQUEST
            elif dbaccess.decline_friend_request(userid_from, userid_to):
                return requeststatus.STATUS_SUCCESS
            else:
                return requeststatus.STATUS_DATABASE_ERROR
        else:
            return requeststatus.STATUS_NON_EXISTENT_USER


def queryfriendship(username1, username2, clients_logged_in):
    if username1 not in clients_logged_in.keys():
        return requeststatus.STATUS_NOT_LOGGED_IN
    else:
        userid1 = dbaccess.get_user_id(username1)
        userid2 = dbaccess.get_user_id(username2)
        if userid1 >= 0 and userid2 >= 0:
            if dbaccess.get_friendship_status(userid1, userid2):
                return requeststatus.QUERYRESPONSE_TRUE
            else:
                return requeststatus.QUERYRESPONSE_FALSE
        else:
            return requeststatus.STATUS_NON_EXISTENT_USER


def queryfriendrequestsent(username1, username2, clients_logged_in):
    if username1 not in clients_logged_in.keys():
        return requeststatus.STATUS_NOT_LOGGED_IN
    else:
        userid1 = dbaccess.get_user_id(username1)
        userid2 = dbaccess.get_user_id(username2)
        if userid1 >= 0 and userid2 >= 0:
            if dbaccess.friend_request_sent(userid1, userid2):
                return requeststatus.QUERYRESPONSE_TRUE
            else:
                return requeststatus.QUERYRESPONSE_FALSE
        else:
            return requeststatus.STATUS_NON_EXISTENT_USER


def queryfriendrequestreceived(username1, username2, clients_logged_in):
    if username1 not in clients_logged_in.keys():
        return requeststatus.STATUS_NOT_LOGGED_IN
    else:
        userid1 = dbaccess.get_user_id(username1)
        userid2 = dbaccess.get_user_id(username2)
        if userid1 >= 0 and userid2 >= 0:
            if dbaccess.friend_request_sent(userid2, userid1):
                return requeststatus.QUERYRESPONSE_TRUE
            else:
                return requeststatus.QUERYRESPONSE_FALSE
        else:
            return requeststatus.STATUS_NON_EXISTENT_USER


def unfriend(username1, username2, clients_logged_in):
    if username1 not in clients_logged_in.keys():
        return requeststatus.STATUS_NOT_LOGGED_IN
    else:
        userid1 = dbaccess.get_user_id(username1)
        userid2 = dbaccess.get_user_id(username2)
        if userid1 >= 0 and userid2 >= 0:
            if dbaccess.get_friendship_status(userid1, userid2):
                if dbaccess.delete_friendship(userid1, userid2):
                    return requeststatus.STATUS_SUCCESS
                else:
                    return requeststatus.STATUS_DATABASE_ERROR
            else:
                return requeststatus.STATUS_NO_FRIENDSHIP
        else:
            return requeststatus.STATUS_NON_EXISTENT_USER


def blockuser(username1, username2, clients_logged_in):
    if username1 not in clients_logged_in.keys():
        return requeststatus.STATUS_NOT_LOGGED_IN
    else:
        userid1 = dbaccess.get_user_id(username1)
        userid2 = dbaccess.get_user_id(username2)
        if userid1 >= 0 and userid2 >= 0:
            if dbaccess.is_user_blocked(userid1, userid2):
                return requeststatus.STATUS_ALREADY_BLOCKED
            elif dbaccess.add_block(userid1, userid2):
                return requeststatus.STATUS_SUCCESS
            else:
                return requeststatus.STATUS_DATABASE_ERROR
        else:
            return requeststatus.STATUS_NON_EXISTENT_USER


def unblockuser(username1, username2, clients_logged_in):
    if username1 not in clients_logged_in.keys():
        return requeststatus.STATUS_NOT_LOGGED_IN
    else:
        userid1 = dbaccess.get_user_id(username1)
        userid2 = dbaccess.get_user_id(username2)
        if userid1 >= 0 and userid2 >= 0:
            if not dbaccess.is_user_blocked(userid1, userid2):
                return requeststatus.STATUS_USER_NOT_BLOCKED
            elif dbaccess.remove_block(userid1, userid2):
                return requeststatus.STATUS_SUCCESS
            else:
                return requeststatus.STATUS_DATABASE_ERROR
        else:
            return requeststatus.STATUS_NON_EXISTENT_USER


def queryblock(username_caller, username1, username2, clients_logged_in):
    if username_caller not in clients_logged_in.keys():
        return requeststatus.STATUS_NOT_LOGGED_IN
    else:
        userid1 = dbaccess.get_user_id(username1)
        userid2 = dbaccess.get_user_id(username2)
        if userid1 >= 0 and userid2 >= 0:
            if dbaccess.is_user_blocked(userid1, userid2):
                return requeststatus.QUERYRESPONSE_TRUE
            else:
                return requeststatus.QUERYRESPONSE_FALSE
        else:
            return requeststatus.STATUS_NON_EXISTENT_USER


def queryfriends(username, clients_logged_in):
    if username not in clients_logged_in.keys():
        return requeststatus.STATUS_NOT_LOGGED_IN
    else:
        userid = dbaccess.get_user_id(username)
        if userid >= 0:
            friendlist = dbaccess.get_friends(userid)
            if friendlist is None:
                return requeststatus.STATUS_SUCCESS, ""
            else:
                friend_list_pretty = ""
                for friend in friendlist:
                    friend_list_pretty += friend[0] + " " + str(friend[1]) + "\n"
                return requeststatus.STATUS_SUCCESS, friend_list_pretty
        else:
            return requeststatus.STATUS_NON_EXISTENT_USER


def queryfriendrequests(username, clients_logged_in):
    if username not in clients_logged_in.keys():
        return requeststatus.STATUS_NOT_LOGGED_IN
    else:
        userid = dbaccess.get_user_id(username)
        if userid >= 0:
            friendrequests = dbaccess.get_friend_requests(userid)
            if friendrequests is None:
                return requeststatus.STATUS_SUCCESS, ""
            else:
                friend_requests_pretty = ""
                for friend_request in friendrequests:
                    friend_requests_pretty += friend_request[0] + "\n"
                return requeststatus.STATUS_SUCCESS, friend_requests_pretty
        else:
            return requeststatus.STATUS_NON_EXISTENT_USER


def changepassword(username, old_pass, new_pass, clients_logged_in):
    if username not in clients_logged_in.keys():
        return requeststatus.STATUS_NOT_LOGGED_IN
    elif len(new_pass) < 5:
        return requeststatus.STATUS_PASSWORD_TOO_SHORT
    elif len(new_pass) > 30:
        return requeststatus.STATUS_PASSWORD_TOO_LONG
    elif dbaccess.user_login(username, old_pass) is None:
        return requeststatus.STATUS_INVALID_PASSWORD
    elif dbaccess.change_password(dbaccess.get_user_id(username), new_pass):
        return requeststatus.STATUS_SUCCESS
    else:
        return requeststatus.STATUS_DATABASE_ERROR


def querydescription(username_caller, username, clients_logged_in):
    if username_caller not in clients_logged_in.keys():
        return requeststatus.STATUS_NOT_LOGGED_IN
    else:
        userid = dbaccess.get_user_id(username)
        if userid >= 0:
            description = dbaccess.get_description(userid)
            if description is None:
                return requeststatus.STATUS_DATABASE_ERROR
            return requeststatus.STATUS_SUCCESS, str(description[0][0])
        else:
            return requeststatus.STATUS_NON_EXISTENT_USER


def setdescription(username, description, clients_logged_in):
    if username not in clients_logged_in.keys():
        return requeststatus.STATUS_NOT_LOGGED_IN
    else:
        userid = dbaccess.get_user_id(username)
        if userid >= 0:
            if not dbaccess.set_description(userid, utils.concatlist(description, ' ')):
                return requeststatus.STATUS_DATABASE_ERROR
            return requeststatus.STATUS_SUCCESS
        else:
            return requeststatus.STATUS_NON_EXISTENT_USER


def queryprofilepic(username_caller, username, clients_logged_in):
    if username_caller not in clients_logged_in.keys():
        return requeststatus.STATUS_NOT_LOGGED_IN
    else:
        userid = dbaccess.get_user_id(username)
        if userid >= 0:
            profile_pic = dbaccess.get_profile_picture(userid)
            if profile_pic is None:
                return requeststatus.STATUS_DATABASE_ERROR
            return requeststatus.STATUS_SUCCESS, len(str(profile_pic[0][0])), str(profile_pic[0][0])
        else:
            return requeststatus.STATUS_NON_EXISTENT_USER


def setprofilepic(username, profile_pic, clients_logged_in):
    if username not in clients_logged_in.keys():
        return requeststatus.STATUS_NOT_LOGGED_IN
    else:
        userid = dbaccess.get_user_id(username)
        if userid >= 0:
            if not dbaccess.set_profile_picture(userid, profile_pic):
                return requeststatus.STATUS_DATABASE_ERROR
            return requeststatus.STATUS_SUCCESS
        else:
            return requeststatus.STATUS_NON_EXISTENT_USER

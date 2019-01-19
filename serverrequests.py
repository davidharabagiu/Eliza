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


def sendmsg(userfrom, timestamp, message, userto, clients_logged_in):
    userto_id = dbaccess.get_user_id(userto)
    if userfrom not in clients_logged_in.keys():
        return requeststatus.STATUS_NOT_LOGGED_IN
    elif len(message) < 1:
        return requeststatus.STATUS_ERROR_EMPTY_MESSAGE
    elif dbaccess.is_user_blocked(userto_id, clients_logged_in[userfrom][1]):
        return requeststatus.STATUS_SENDER_BLOCKED
    elif dbaccess.is_user_blocked(clients_logged_in[userfrom][1], userto_id):
        return requeststatus.STATUS_RECEIVER_BLOCKED
    else:
        userfrom_id = dbaccess.get_user_id(userfrom)
        userto_id = dbaccess.get_user_id(userto)
        content = utils.concatlist(message, ' ')
        dbaccess.insert_message(timestamp, userfrom_id, userto_id, content)
        if userto in clients_logged_in.keys():
            clients_logged_in[userto][0].sendall(userfrom + ':' + content)
        return requeststatus.STATUS_SUCCESS


def getmessages(username1, username2):
    userid1 = dbaccess.get_user_id(username1)
    userid2 = dbaccess.get_user_id(username2)
    if userid1 >= 0 and userid2 >= 0:
        messages = dbaccess.get_messages(userid1, userid2)
        messages_pretty = ""
        if not (messages is None):
            for msg in messages:
                messages_pretty += msg[0] + " " + msg[1] + " " + msg[2] + "\n"
        return requeststatus.STATUS_SUCCESS, messages_pretty
    else:
        return requeststatus.STATUS_NON_EXISTENT_USER


def sendsong(userfrom, song, userto, clients_logged_in):
    if userfrom not in clients_logged_in.keys():
        return requeststatus.STATUS_NOT_LOGGED_IN
    elif userto not in clients_logged_in.keys():
        return requeststatus.STATUS_USER_NOT_ONLINE
    elif len(song) < 1:
        return requeststatus.STATUS_EMPTY_SONG_NAME
    elif dbaccess.is_user_blocked(clients_logged_in[userto][1], clients_logged_in[userfrom][1]):
        return requeststatus.STATUS_SENDER_BLOCKED
    elif dbaccess.is_user_blocked(clients_logged_in[userfrom][1], clients_logged_in[userto][1]):
        return requeststatus.STATUS_RECEIVER_BLOCKED
    else:
        song_s = utils.concatlist(song, ' ')
        dbdata = dbaccess.get_song_data(song_s[:len(song_s) - 1])
        if dbdata is None:
            return requeststatus.STATUS_INVALID_SONG
        song_data = str(dbdata[0][0])
        clients_logged_in[userto][0].sendall('/song ' + str(len(song_data)))
        file_bytes_sent = 0
        file_bytes_to_send = len(song_data)
        while file_bytes_sent < file_bytes_to_send:
            clients_logged_in[userto][0].sendall(song_data[:min(1024, len(song_data))])
            if len(song_data) <= 1024:
                song_data = ''
            else:
                song_data = song_data[1024:]
            file_bytes_sent += 1024
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


def createroom(username, room_name, public, clients_logged_in):
    if username not in clients_logged_in.keys():
        return requeststatus.STATUS_NOT_LOGGED_IN
    else:
        userid = dbaccess.get_user_id(username)
        roomid = dbaccess.get_room_id(room_name)
        if roomid < 0:
            dbaccess.createroom(userid, room_name, int(public))
            return requeststatus.STATUS_SUCCESS
        else:
            return requeststatus.STATUS_DATABASE_ERROR


def deleteroom(username, room_name, clients_logged_in):
    if username not in clients_logged_in.keys():
        return requeststatus.STATUS_NOT_LOGGED_IN
    else:
        userid = dbaccess.get_user_id(username)
        roomid = dbaccess.get_room_id(room_name)
        if roomid >= 0:
            ownerid = dbaccess.get_room_owner(room_name)
            if ownerid != userid:
                return requeststatus.STATUS_NOT_ALLOWED
            dbaccess.deleteroom(room_name)
            return requeststatus.STATUS_SUCCESS
        else:
            return requeststatus.STATUS_DATABASE_ERROR


def addtoroom(username, room_name, member_username, clients_logged_in):
    if username not in clients_logged_in.keys():
        return requeststatus.STATUS_NOT_LOGGED_IN
    else:
        userid = dbaccess.get_user_id(username)
        memberid = dbaccess.get_user_id(member_username)
        roomid = dbaccess.get_room_id(room_name)
        if roomid >= 0:
            if dbaccess.ismember(memberid, roomid):
                return requeststatus.STATUS_SUCCESS
            ownerid = dbaccess.get_room_owner(room_name)
            private = dbaccess.is_room_private(room_name)
            if ownerid != userid and private:
                return requeststatus.STATUS_NOT_ALLOWED
            if memberid < 0:
                return requeststatus.STATUS_NON_EXISTENT_USER
            dbaccess.addtoroom(memberid, roomid)
            return requeststatus.STATUS_SUCCESS
        else:
            return requeststatus.STATUS_DATABASE_ERROR


def kickfromroom(username, room_name, member_username, clients_logged_in):
    if username not in clients_logged_in.keys():
        return requeststatus.STATUS_NOT_LOGGED_IN
    else:
        userid = dbaccess.get_user_id(username)
        roomid = dbaccess.get_room_id(room_name)
        if roomid >= 0:
            memberid = dbaccess.get_user_id(member_username)
            ownerid = dbaccess.get_room_owner(room_name)
            if memberid < 0:
                return requeststatus.STATUS_NON_EXISTENT_USER
            if ownerid != userid and userid != memberid:
                return requeststatus.STATUS_NOT_ALLOWED
            dbaccess.kickfromroom(memberid, roomid)
            return requeststatus.STATUS_SUCCESS
        else:
            return requeststatus.STATUS_DATABASE_ERROR


def getrooms(username, clients_logged_in):
    if username not in clients_logged_in.keys():
        return requeststatus.STATUS_NOT_LOGGED_IN
    else:
        userid = dbaccess.get_user_id(username)
        roomlist = dbaccess.getrooms(userid)
        if roomlist is None:
            return requeststatus.STATUS_SUCCESS, ""
        else:
            room_list_pretty = ""
            for room in roomlist:
                room_list_pretty += room[0] + "\n"
            return requeststatus.STATUS_SUCCESS, room_list_pretty


def getroommessages(username, room_name, clients_logged_in):
    if username not in clients_logged_in.keys():
        return requeststatus.STATUS_NOT_LOGGED_IN
    userid = dbaccess.get_user_id(username)
    roomid = dbaccess.get_room_id(room_name)
    if roomid < 0:
        return requeststatus.STATUS_DATABASE_ERROR
    if dbaccess.ismember(userid, roomid):
        messages = dbaccess.get_room_messages(roomid)
        messages_pretty = ""
        if not (messages is None):
            for msg in messages:
                messages_pretty += msg[0] + " " + msg[1] + " " + msg[2] + "\n"
        return requeststatus.STATUS_SUCCESS, messages_pretty
    else:
        return requeststatus.STATUS_NOT_ALLOWED


def doiownthis(username, room_name, clients_logged_in):
    if username not in clients_logged_in.keys():
        return requeststatus.STATUS_NOT_LOGGED_IN
    userid = dbaccess.get_user_id(username)
    roomid = dbaccess.get_room_id(room_name)
    if roomid < 0:
        return requeststatus.STATUS_DATABASE_ERROR
    if dbaccess.ismember(userid, roomid):
        owner = dbaccess.get_room_owner(room_name)
        if owner == userid:
            return requeststatus.STATUS_SUCCESS, "yes"
        return requeststatus.STATUS_SUCCESS, "no"
    else:
        return requeststatus.STATUS_NOT_ALLOWED


def getroommembers(username, room_name, clients_logged_in):
    if username not in clients_logged_in.keys():
        return requeststatus.STATUS_NOT_LOGGED_IN
    userid = dbaccess.get_user_id(username)
    roomid = dbaccess.get_room_id(room_name)
    if roomid < 0:
        return requeststatus.STATUS_DATABASE_ERROR
    if dbaccess.ismember(userid, roomid):
        members = dbaccess.get_member_names(roomid)
        members_pretty = ""
        if not (members is None):
            for member in members:
                if member[0] in clients_logged_in.keys():
                    members_pretty += member[0] + " 1\n"
                else:
                    members_pretty += member[0] + " 0\n"
        return requeststatus.STATUS_SUCCESS, members_pretty
    else:
        return requeststatus.STATUS_NOT_ALLOWED


def broadcastmsg(username, timestamp, message, room_name, clients_logged_in):
    userid = dbaccess.get_user_id(username)
    roomid = dbaccess.get_room_id(room_name)
    if username not in clients_logged_in.keys():
        return requeststatus.STATUS_NOT_LOGGED_IN
    elif len(message) < 1:
        return requeststatus.STATUS_ERROR_EMPTY_MESSAGE
    elif roomid < 0:
        return requeststatus.STATUS_DATABASE_ERROR
    elif not dbaccess.ismember(userid, roomid):
        return requeststatus.STATUS_NOT_ALLOWED
    else:
        content = utils.concatlist(message, ' ')
        dbaccess.insert_room_message(timestamp, userid, roomid, content)
        members = dbaccess.get_member_names(roomid)
        print clients_logged_in.keys()
        print members
        for member in members:
            if member[0] in clients_logged_in.keys() and member[0] != username:
                clients_logged_in[member[0]][0].sendall(room_name + "#" + username + ':' + content)
        return requeststatus.STATUS_SUCCESS

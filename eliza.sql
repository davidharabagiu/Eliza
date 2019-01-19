create database if not exists eliza;
use eliza;

create table if not exists accounts(
	account_id int not null auto_increment,
	user_name varchar(50) not null,
	password varchar(50) not null,
	profile_pic mediumtext,
	description tinytext,
	online_status tinyint not null,
	primary key(account_id)
);

create table if not exists friendrequests(
  user_from int not null,
  user_to int not null,
  primary key(user_from, user_to),
  foreign key(user_from) references accounts(account_id),
  foreign key(user_to) references accounts(account_id)
);

create table if not exists friendships(
	user1 int not null,
	user2 int not null,
	primary key(user1, user2),
	foreign key(user1) references accounts(account_id),
	foreign key(user2) references accounts(account_id)
);

create table if not exists songs(
	song_name varchar(100) not null,
	mp3_data mediumblob not null,
	primary key(song_name)
);

create table if not exists blocks(
  user1 int not null,
  user2 int not null,
  primary key(user1, user2),
  foreign key(user1) references accounts(account_id),
  foreign key(user2) references accounts(account_id)
);

create table if not exists chatrooms(
	id int not null auto_increment,
    name varchar(30) not null,
    owner int not null,
    public bool not null,
    primary key (id),
    foreign key (owner) references accounts(account_id)
);

create table if not exists chatroom_messages(
	id int not null auto_increment,
    timestamp char(18) not null,
	user int not null,
    chatroom int not null,
    content varchar (150) not null,
    replied_to int,
    primary key(id),
    foreign key(user) references accounts(account_id),
    foreign key(chatroom) references chatrooms(id),
    foreign key(replied_to) references chatroom_messages(id)
);

create table if not exists chatroom_memberships(
	user int not null,
    chatroom int not null,
    primary key(user, chatroom),
    foreign key(user) references accounts(account_id),
    foreign key(chatroom) references chatrooms(id)
);

create table if not exists messages(
	id int not null auto_increment,
    timestamp char(18) not null,
	user1 int not null,
    user2 int not null,
    content varchar (150) not null,
    primary key(id),
    foreign key(user1) references accounts(account_id),
    foreign key(user2) references accounts(account_id)
);

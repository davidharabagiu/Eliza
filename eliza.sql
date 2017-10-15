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
	song_id int not null,
	song_name varchar(100) not null,
	mp3_data mediumblob not null,
	primary key(song_id)
);

create table if not exists favorite_songs(
	account_id int not null,
	song_id int not null,
	primary key(account_id, song_id),
	foreign key(account_id) references accounts(account_id),
	foreign key(song_id) references songs(song_id)
);

create table if not exists blocks(
  user1 int not null,
  user2 int not null,
  primary key(user1, user2),
  foreign key(user1) references accounts(account_id),
  foreign key(user2) references accounts(account_id)
);

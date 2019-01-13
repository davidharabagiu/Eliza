import sys
import os
import base64
import mysql.connector


connection_config = {
    'user': 'root',
    'password': 'root',
    'host': '127.0.0.1',
    'database': 'eliza',
}


def to_base_64(file_path):
    fh = open(file_path, 'rb')
    file_data = fh.read()
    fh.close()
    return base64.encodestring(file_data)


def add_song_to_db(song_path):
    song_name = os.path.splitext(os.path.basename(song_path))[0]
    mp3_data = to_base_64(song_path)

    try:
        connection = mysql.connector.connect(**connection_config)
        cursor = connection.cursor(buffered=True)
        cursor.execute("INSERT INTO songs (song_name, mp3_data) VALUES (%s, %s)", (song_name, mp3_data))
        connection.commit()
        cursor.close()
        connection.close()
        return True
    except mysql.connector.errors.Error as err:
        print err
        return False


if __name__ == '__main__':

    if len(sys.argv) < 2:
        print('Usage: {} music_dir/music_file'.format(sys.argv[0]))
        exit(0)

    song_list = list()

    if os.path.isfile(sys.argv[1]):
        song_list.append(sys.argv[1])
    elif os.path.isdir(sys.argv[1]):
        for f in os.listdir(sys.argv[1]):
            song_list.append(os.path.join(sys.argv[1], f))

    for song in song_list:
        print('Adding {}'.format(song))
        add_song_to_db(song)

CREATE TABLE test_db(
id int IDENTITY(1,1) NOT NULL,
type_of varchar(1000) NOT NULL,
count_of int NOT NULL,
postavka varchar(50) NOT NULL,
price int NOT NULL,
PRIMARY KEY(id)
);
--liquibase formatted sql

--changeset you:002

insert into cinemasales.seats_list (number, cost) values ('A1', 300);
insert into cinemasales.seats_list (number, cost) values ('A2', 300);
insert into cinemasales.seats_list (number, cost) values ('A3', 300);
insert into cinemasales.seats_list (number, cost) values ('A4', 300);
insert into cinemasales.seats_list (number, cost) values ('A5', 300);
insert into cinemasales.seats_list (number, cost) values ('A6', 300);
insert into cinemasales.seats_list (number, cost) values ('A7', 300);
insert into cinemasales.seats_list (number, cost) values ('A8', 300);
insert into cinemasales.seats_list (number, cost) values ('A9', 300);
insert into cinemasales.seats_list (number, cost) values ('B1', 500);
insert into cinemasales.seats_list (number, cost) values ('B2', 500);
insert into cinemasales.seats_list (number, cost) values ('B3', 500);
insert into cinemasales.seats_list (number, cost) values ('B4', 500);
insert into cinemasales.seats_list (number, cost) values ('B5', 500);
insert into cinemasales.seats_list (number, cost) values ('B6', 500);
insert into cinemasales.seats_list (number, cost) values ('B7', 500);
insert into cinemasales.seats_list (number, cost) values ('B8', 500);
insert into cinemasales.seats_list (number, cost) values ('B9', 500);
insert into cinemasales.seats_list (number, cost) values ('C1', 750);
insert into cinemasales.seats_list (number, cost) values ('C2', 750);
insert into cinemasales.seats_list (number, cost) values ('C3', 750);
insert into cinemasales.seats_list (number, cost) values ('C4', 750);
insert into cinemasales.seats_list (number, cost) values ('C5', 750);
insert into cinemasales.seats_list (number, cost) values ('C6', 750);
insert into cinemasales.seats_list (number, cost) values ('C7', 750);
insert into cinemasales.seats_list (number, cost) values ('C8', 750);
insert into cinemasales.seats_list (number, cost) values ('C9', 750);

INSERT INTO booked_seats.all_seats (number) values ('A1');
INSERT INTO booked_seats.all_seats (number) values ('A2');
INSERT INTO booked_seats.all_seats (number) values ('A3');
INSERT INTO booked_seats.all_seats (number) values ('A4');
INSERT INTO booked_seats.all_seats (number) values ('A5');
INSERT INTO booked_seats.all_seats (number) values ('A6');
INSERT INTO booked_seats.all_seats (number) values ('A7');
INSERT INTO booked_seats.all_seats (number) values ('A8');
INSERT INTO booked_seats.all_seats (number) values ('A9');
INSERT INTO booked_seats.all_seats (number) values ('B1');
INSERT INTO booked_seats.all_seats (number) values ('B2');
INSERT INTO booked_seats.all_seats (number) values ('B3');
INSERT INTO booked_seats.all_seats (number) values ('B4');
INSERT INTO booked_seats.all_seats (number) values ('B5');
INSERT INTO booked_seats.all_seats (number) values ('B6');
INSERT INTO booked_seats.all_seats (number) values ('B7');
INSERT INTO booked_seats.all_seats (number) values ('B8');
INSERT INTO booked_seats.all_seats (number) values ('B9');
INSERT INTO booked_seats.all_seats (number) values ('C1');
INSERT INTO booked_seats.all_seats (number) values ('C2');
INSERT INTO booked_seats.all_seats (number) values ('C3');
INSERT INTO booked_seats.all_seats (number) values ('C4');
INSERT INTO booked_seats.all_seats (number) values ('C5');
INSERT INTO booked_seats.all_seats (number) values ('C6');
INSERT INTO booked_seats.all_seats (number) values ('C7');
INSERT INTO booked_seats.all_seats (number) values ('C8');
INSERT INTO booked_seats.all_seats (number) values ('C9');

INSERT INTO wallet.users_wallets (username, wallet, pincode) values ('user', 1000000, '0000');
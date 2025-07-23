--liquibase formatted sql

--changeset you:001

create schema cinemasales; 

create table cinemasales.seats_list
(
    id smallserial primary key,
    number text not null,
    cost int not null default 300
);

create table cinemasales.orders_list(
    id serial primary key,
    seat_number text not null,
    username text not null,
    token text not null,
    status smallint not null default 0
);

create table cinemasales.orders_result_list(
    id serial primary key,
    token text not null unique,
    pay_status smallint default 0,
    book_status smallint default 0,
    result_status smallint default 0,
    seat_number text,
    username text,
    cost int,
    pincode text
);

create schema booked_seats;

create table booked_seats.all_seats
(
    id smallserial primary key,
    number text not null,
    status smallint not null default '1'
);

create schema wallet;

create table wallet.users_wallets
(
    id serial primary key,
    username text not null,
    wallet int not null default 2000,
    pincode text not null 
)
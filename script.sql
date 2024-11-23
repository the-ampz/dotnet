---- DROP TABLES PRINCIPAIS
--DROP TABLE t_ampz_score CASCADE CONSTRAINTS;
--DROP TABLE t_ampz_energy_consumption CASCADE CONSTRAINTS;
--DROP TABLE t_ampz_challenge_goal CASCADE CONSTRAINTS;
--DROP TABLE t_ampz_community_participation CASCADE CONSTRAINTS;
--DROP TABLE t_ampz_community CASCADE CONSTRAINTS;
--DROP TABLE t_ampz_device CASCADE CONSTRAINTS;
--DROP TABLE t_ampz_kid CASCADE CONSTRAINTS;
--DROP TABLE t_ampz_user CASCADE CONSTRAINTS;
--DROP TABLE t_ampz_address CASCADE CONSTRAINTS;
--
---- DROP TABLES DE AUDITORIA
--DROP TABLE t_audit_ampz_address;
--DROP TABLE t_audit_ampz_user;
--DROP TABLE t_audit_ampz_kid;
--DROP TABLE t_audit_ampz_device;
--DROP TABLE t_audit_ampz_community;
--DROP TABLE t_audit_ampz_community_participation;
--DROP TABLE t_audit_ampz_challenge_goal;
--DROP TABLE t_audit_ampz_energy_consumption;
--DROP TABLE t_audit_ampz_score;
--
---- DROP SEQUENCES PRINCIPAIS
--DROP SEQUENCE seq_t_ampz_score;
--DROP SEQUENCE seq_t_ampz_energy_consumption;
--DROP SEQUENCE seq_t_ampz_challenge_goal;
--DROP SEQUENCE seq_t_ampz_community_participation;
--DROP SEQUENCE seq_t_ampz_community;
--DROP SEQUENCE seq_t_ampz_device;
--DROP SEQUENCE seq_t_ampz_kid;
--DROP SEQUENCE seq_t_ampz_user;
--DROP SEQUENCE seq_t_ampz_address;
--
---- DROP SEQUENCES DE AUDITORIA
--DROP SEQUENCE seq_t_audit_ampz_address;
--DROP SEQUENCE seq_t_audit_ampz_user;
--DROP SEQUENCE seq_t_audit_ampz_kid;
--DROP SEQUENCE seq_t_audit_ampz_device;
--DROP SEQUENCE seq_t_audit_ampz_community;
--DROP SEQUENCE seq_t_audit_ampz_community_participation;
--DROP SEQUENCE seq_t_audit_ampz_challenge_goal;
--DROP SEQUENCE seq_t_audit_ampz_energy_consumption;
--DROP SEQUENCE seq_t_audit_ampz_score;

-- TABELAS PRINCIPAIS
CREATE TABLE t_ampz_address (
    id_address    NUMBER PRIMARY KEY,
    ds_street     VARCHAR2(100),
    ds_number     VARCHAR2(10),
    ds_complement VARCHAR2(50),
    ds_district   VARCHAR2(50),
    ds_city       VARCHAR2(50),
    ds_state      VARCHAR2(50)
);

CREATE TABLE t_ampz_user (
    id_user      NUMBER PRIMARY KEY,
    ds_name      VARCHAR2(100),
    ds_email     VARCHAR2(100) UNIQUE,
    ds_password  VARCHAR2(100),
    id_address   NUMBER,
    dt_birthdate DATE,
    CONSTRAINT fk_user_address FOREIGN KEY (id_address)
        REFERENCES t_ampz_address (id_address)
);

CREATE TABLE t_ampz_kid (
    id_kid             NUMBER PRIMARY KEY,
    id_user            NUMBER,
    ds_name            VARCHAR2(100),
    dt_birthdate       DATE,
    total_score        NUMBER DEFAULT 0,
    total_energy_saved NUMBER DEFAULT 0,
    CONSTRAINT fk_kid_user FOREIGN KEY (id_user)
        REFERENCES t_ampz_user (id_user)
);

CREATE TABLE t_ampz_device (
    id_device             NUMBER PRIMARY KEY,
    id_kid                NUMBER,
    ds_name               VARCHAR2(100),
    ds_type               VARCHAR2(50),
    ds_operating_system   VARCHAR2(50),
    vl_energy_consumption NUMBER,
    vl_energy_saved       NUMBER,
    CONSTRAINT fk_device_kid FOREIGN KEY (id_kid)
        REFERENCES t_ampz_kid (id_kid)
);

CREATE TABLE t_ampz_community (
    id_community   NUMBER PRIMARY KEY,
    ds_name        VARCHAR2(100),
    ds_description CLOB,
    total_points   NUMBER DEFAULT 0
);

CREATE TABLE t_ampz_community_participation (
    id_participation NUMBER PRIMARY KEY,
    id_kid           NUMBER,
    id_community     NUMBER,
    points           NUMBER DEFAULT 0,
    CONSTRAINT fk_participation_kid FOREIGN KEY (id_kid)
        REFERENCES t_ampz_kid (id_kid),
    CONSTRAINT fk_participation_community FOREIGN KEY (id_community)
        REFERENCES t_ampz_community (id_community)
);

CREATE TABLE t_ampz_challenge_goal (
    id_challenge       NUMBER PRIMARY KEY,
    ds_description     CLOB,
    vl_score           NUMBER,
    dt_start           DATE,
    dt_end             DATE,
    vl_energy_required NUMBER,
    id_community       NUMBER,
    CONSTRAINT fk_challenge_community FOREIGN KEY (id_community)
        REFERENCES t_ampz_community (id_community)
);

CREATE TABLE t_ampz_energy_consumption (
    id_energy_consumption NUMBER PRIMARY KEY,
    id_device             NUMBER,
    ds_consumption_type   VARCHAR2(50),
    vl_consumption        NUMBER,
    vl_energy_saved       NUMBER,
    dt_consumption        DATE,
    CONSTRAINT fk_energy_consumption_device FOREIGN KEY (id_device)
        REFERENCES t_ampz_device (id_device)
);

CREATE TABLE t_ampz_score (
    id_score      NUMBER PRIMARY KEY,
    id_kid        NUMBER,
    id_challenge  NUMBER,
    vl_points     NUMBER,
    dt_completion DATE,
    CONSTRAINT fk_score_kid FOREIGN KEY (id_kid)
        REFERENCES t_ampz_kid (id_kid),
    CONSTRAINT fk_score_challenge FOREIGN KEY (id_challenge)
        REFERENCES t_ampz_challenge_goal (id_challenge)
);

-- TABELAS DE AUDITORIA
CREATE TABLE t_audit_ampz_address (
    audit_id NUMBER PRIMARY KEY,
    id_address NUMBER,
    operation VARCHAR2(20),
    operation_date TIMESTAMP,
    old_street VARCHAR2(100),
    old_number VARCHAR2(10),
    new_street VARCHAR2(100),
    new_number VARCHAR2(10),
    CONSTRAINT fk_audit_address FOREIGN KEY (id_address)
        REFERENCES t_ampz_address (id_address)
);

CREATE TABLE t_audit_ampz_user (
    audit_id NUMBER PRIMARY KEY,
    id_user NUMBER,
    operation VARCHAR2(20),
    operation_date TIMESTAMP,
    old_name VARCHAR2(100),
    old_email VARCHAR2(100),
    new_name VARCHAR2(100),
    new_email VARCHAR2(100),
    CONSTRAINT fk_audit_user FOREIGN KEY (id_user)
        REFERENCES t_ampz_user (id_user)
);

CREATE TABLE t_audit_ampz_kid (
    audit_id NUMBER PRIMARY KEY,
    id_kid NUMBER,
    operation VARCHAR2(20),
    operation_date TIMESTAMP,
    old_name VARCHAR2(100),
    old_total_score NUMBER,
    new_name VARCHAR2(100),
    new_total_score NUMBER,
    CONSTRAINT fk_audit_kid FOREIGN KEY (id_kid)
        REFERENCES t_ampz_kid (id_kid)
);

CREATE TABLE t_audit_ampz_device (
    audit_id NUMBER PRIMARY KEY,
    id_device NUMBER,
    id_kid NUMBER,
    operation VARCHAR2(20),
    operation_date TIMESTAMP,
    old_name VARCHAR2(100),
    old_energy_consumption NUMBER,
    new_name VARCHAR2(100),
    new_energy_consumption NUMBER,
    CONSTRAINT fk_audit_device FOREIGN KEY (id_device)
        REFERENCES t_ampz_device (id_device)
);

CREATE TABLE t_audit_ampz_community (
    audit_id NUMBER PRIMARY KEY,
    id_community NUMBER,
    operation VARCHAR2(20),
    operation_date TIMESTAMP,
    old_name VARCHAR2(100),
    old_total_points NUMBER,
    new_name VARCHAR2(100),
    new_total_points NUMBER,
    CONSTRAINT fk_audit_community FOREIGN KEY (id_community)
        REFERENCES t_ampz_community (id_community)
);

CREATE TABLE t_audit_ampz_community_participation (
    audit_id NUMBER PRIMARY KEY,
    id_participation NUMBER,
    id_kid NUMBER,
    id_community NUMBER,
    operation VARCHAR2(20),
    operation_date TIMESTAMP,
    old_points NUMBER,
    new_points NUMBER,
    CONSTRAINT fk_audit_participation FOREIGN KEY (id_participation)
        REFERENCES t_ampz_community_participation (id_participation),
    CONSTRAINT fk_audit_participation_kid FOREIGN KEY (id_kid)
        REFERENCES t_ampz_kid (id_kid),
    CONSTRAINT fk_audit_participation_community FOREIGN KEY (id_community)
        REFERENCES t_ampz_community (id_community)
);

CREATE TABLE t_audit_ampz_challenge_goal (
    audit_id NUMBER PRIMARY KEY,
    id_challenge NUMBER,
    id_community NUMBER,
    operation VARCHAR2(20),
    operation_date TIMESTAMP,
    old_description CLOB,
    old_score NUMBER,
    new_description CLOB,
    new_score NUMBER,
    CONSTRAINT fk_audit_challenge FOREIGN KEY (id_challenge)
        REFERENCES t_ampz_challenge_goal (id_challenge),
    CONSTRAINT fk_audit_challenge_community FOREIGN KEY (id_community)
        REFERENCES t_ampz_community (id_community)
);

CREATE TABLE t_audit_ampz_energy_consumption (
    audit_id NUMBER PRIMARY KEY,
    id_energy_consumption NUMBER,
    id_device NUMBER,
    operation VARCHAR2(20),
    operation_date TIMESTAMP,
    old_consumption_type VARCHAR2(50),
    old_energy_saved NUMBER,
    new_consumption_type VARCHAR2(50),
    new_energy_saved NUMBER,
    CONSTRAINT fk_audit_energy_consumption FOREIGN KEY (id_energy_consumption)
        REFERENCES t_ampz_energy_consumption (id_energy_consumption),
    CONSTRAINT fk_audit_energy_consumption_device FOREIGN KEY (id_device)
        REFERENCES t_ampz_device (id_device)
);

CREATE TABLE t_audit_ampz_score (
    audit_id NUMBER PRIMARY KEY,
    id_score NUMBER,
    id_kid NUMBER,
    id_challenge NUMBER,
    operation VARCHAR2(20),
    operation_date TIMESTAMP,
    old_points NUMBER,
    new_points NUMBER,
    CONSTRAINT fk_audit_score FOREIGN KEY (id_score)
        REFERENCES t_ampz_score (id_score),
    CONSTRAINT fk_audit_score_kid FOREIGN KEY (id_kid)
        REFERENCES t_ampz_kid (id_kid),
    CONSTRAINT fk_audit_score_challenge FOREIGN KEY (id_challenge)
        REFERENCES t_ampz_challenge_goal (id_challenge)
);

-- SEQUENCES PRINCIPAIS
CREATE SEQUENCE seq_t_ampz_address START WITH 1 INCREMENT BY 1 MAXVALUE 9999999 NOCYCLE;
CREATE SEQUENCE seq_t_ampz_user START WITH 1 INCREMENT BY 1 MAXVALUE 9999999 NOCYCLE;
CREATE SEQUENCE seq_t_ampz_kid START WITH 1 INCREMENT BY 1 MAXVALUE 9999999 NOCYCLE;
CREATE SEQUENCE seq_t_ampz_device START WITH 1 INCREMENT BY 1 MAXVALUE 9999999 NOCYCLE;
CREATE SEQUENCE seq_t_ampz_community START WITH 1 INCREMENT BY 1 MAXVALUE 9999999 NOCYCLE;
CREATE SEQUENCE seq_t_ampz_community_participation START WITH 1 INCREMENT BY 1 MAXVALUE 9999999 NOCYCLE;
CREATE SEQUENCE seq_t_ampz_challenge_goal START WITH 1 INCREMENT BY 1 MAXVALUE 9999999 NOCYCLE;
CREATE SEQUENCE seq_t_ampz_energy_consumption START WITH 1 INCREMENT BY 1 MAXVALUE 9999999 NOCYCLE;
CREATE SEQUENCE seq_t_ampz_score START WITH 1 INCREMENT BY 1 MAXVALUE 9999999 NOCYCLE;

-- SEQUENCES DE AUDITORIA
CREATE SEQUENCE seq_t_audit_ampz_address START WITH 1 INCREMENT BY 1 MAXVALUE 9999999 NOCYCLE;
CREATE SEQUENCE seq_t_audit_ampz_user START WITH 1 INCREMENT BY 1 MAXVALUE 9999999 NOCYCLE;
CREATE SEQUENCE seq_t_audit_ampz_kid START WITH 1 INCREMENT BY 1 MAXVALUE 9999999 NOCYCLE;
CREATE SEQUENCE seq_t_audit_ampz_device START WITH 1 INCREMENT BY 1 MAXVALUE 9999999 NOCYCLE;
CREATE SEQUENCE seq_t_audit_ampz_community START WITH 1 INCREMENT BY 1 MAXVALUE 9999999 NOCYCLE;
CREATE SEQUENCE seq_t_audit_ampz_community_participation START WITH 1 INCREMENT BY 1 MAXVALUE 9999999 NOCYCLE;
CREATE SEQUENCE seq_t_audit_ampz_challenge_goal START WITH 1 INCREMENT BY 1 MAXVALUE 9999999 NOCYCLE;
CREATE SEQUENCE seq_t_audit_ampz_energy_consumption START WITH 1 INCREMENT BY 1 MAXVALUE 9999999 NOCYCLE;
CREATE SEQUENCE seq_t_audit_ampz_score START WITH 1 INCREMENT BY 1 MAXVALUE 9999999 NOCYCLE;




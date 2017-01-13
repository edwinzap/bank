Use bank;

DROP TABLE Extrait
DROP TABLE Beneficiaire

CREATE TABLE Extrait(
numSequence int NOT NULL,
anneeSequence int NOT NULL,
montant decimal (6,2) NOT NULL,
devise int DEFAULT 0,
dateExecution date NOT NULL,
dateValeur date NOT NULL,
communication varchar(250),
mandat varchar(100),
reference varchar(100),
details varchar(500),
avecCarte bit DEFAULT 0,
id_beneficiaire int NOT NULL
PRIMARY KEY (numSequence, anneeSequence))


CREATE TABLE Beneficiaire(
id int PRIMARY KEY IDENTITY,
nom varchar(100) NOT NULL,
compte varchar(25),
adresse varchar(250))

ALTER TABLE Extrait
ADD CONSTRAINT fk_extrait_beneficiaire FOREIGN KEY (id_beneficiaire) REFERENCES beneficiaire(id);
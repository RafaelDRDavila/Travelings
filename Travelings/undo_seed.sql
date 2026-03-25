DELETE FROM produtos WHERE id > 3;
DELETE FROM lojas WHERE id > 8;
UPDATE lojas SET descricao='', logo='', banner='', email='', telefone='', endereco='' WHERE id = 8;
ALTER TABLE produtos ALTER COLUMN nome TYPE varchar(30);
ALTER TABLE produtos ALTER COLUMN imagem TYPE varchar(500);

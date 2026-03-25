-- ═══ PRAIA ═══
-- Proteção
UPDATE produtos SET subcategoria = 'Proteção' WHERE id = 40; -- Protetor Solar FPS 70
UPDATE produtos SET subcategoria = 'Proteção' WHERE id = 49; -- Oculos de Sol Polarizado
UPDATE produtos SET subcategoria = 'Proteção' WHERE id = 48; -- Bone Quiksilver Trucker

-- Conforto
UPDATE produtos SET subcategoria = 'Conforto' WHERE id = 45; -- Cadeira de Praia Reclinavel
UPDATE produtos SET subcategoria = 'Conforto' WHERE id = 41; -- Canga Toalha Microfibra
UPDATE produtos SET subcategoria = 'Conforto' WHERE id = 42; -- Guarda-Sol UV40

-- Diversão
UPDATE produtos SET subcategoria = 'Diversão' WHERE id = 43; -- Kit Snorkel Adulto
UPDATE produtos SET subcategoria = 'Diversão' WHERE id = 44; -- Prancha Bodyboard
UPDATE produtos SET subcategoria = 'Diversão' WHERE id = 50; -- Boia Inflavel

-- Acessórios
UPDATE produtos SET subcategoria = 'Acessórios' WHERE id = 39; -- Chinelo Havaianas
UPDATE produtos SET subcategoria = 'Acessórios' WHERE id = 47; -- Boardshort Quiksilver

-- Hidratação
UPDATE produtos SET subcategoria = 'Hidratação' WHERE id = 46; -- Bolsa Termica 20L

-- ═══ ACAMPAMENTO ═══
-- Abrigo
UPDATE produtos SET subcategoria = 'Abrigo' WHERE id = 1;  -- Barraca
UPDATE produtos SET subcategoria = 'Abrigo' WHERE id = 51; -- Barraca Nautika Cherokee
UPDATE produtos SET subcategoria = 'Abrigo' WHERE id = 59; -- Rede de Descanso

-- Iluminação
UPDATE produtos SET subcategoria = 'Iluminação' WHERE id = 2;  -- Laterna
UPDATE produtos SET subcategoria = 'Iluminação' WHERE id = 53; -- Lanterna LED 1000lm
UPDATE produtos SET subcategoria = 'Iluminação' WHERE id = 61; -- Lampiao Solar

-- Cozinha
UPDATE produtos SET subcategoria = 'Cozinha' WHERE id = 54; -- Fogareiro Compacto
UPDATE produtos SET subcategoria = 'Cozinha' WHERE id = 57; -- Kit Panelas Camping
UPDATE produtos SET subcategoria = 'Cozinha' WHERE id = 62; -- Garrafa Termica 1L

-- Equipamento
UPDATE produtos SET subcategoria = 'Equipamento' WHERE id = 3;  -- Saco de Dormir Premium
UPDATE produtos SET subcategoria = 'Equipamento' WHERE id = 52; -- Saco de Dormir Confort
UPDATE produtos SET subcategoria = 'Equipamento' WHERE id = 55; -- Colchonete Inflavel
UPDATE produtos SET subcategoria = 'Equipamento' WHERE id = 56; -- Mochila Cargueira 60L
UPDATE produtos SET subcategoria = 'Equipamento' WHERE id = 60; -- Cadeira Dobravel

-- Segurança
UPDATE produtos SET subcategoria = 'Segurança' WHERE id = 58; -- Canivete Multiuso

-- ═══ TURISMO ═══
-- Eletrônicos
UPDATE produtos SET subcategoria = 'Eletrônicos' WHERE id = 67; -- Adaptador Universal Tomada
UPDATE produtos SET subcategoria = 'Eletrônicos' WHERE id = 71; -- Tag Rastreador Bluetooth

-- Organização
UPDATE produtos SET subcategoria = 'Organização' WHERE id = 63; -- Mala Samsonite Spinner
UPDATE produtos SET subcategoria = 'Organização' WHERE id = 64; -- Mala de Bordo
UPDATE produtos SET subcategoria = 'Organização' WHERE id = 69; -- Organizador de Malas

-- Segurança
UPDATE produtos SET subcategoria = 'Segurança' WHERE id = 65; -- Mochila Anti-Furto
UPDATE produtos SET subcategoria = 'Segurança' WHERE id = 70; -- Cadeado TSA
UPDATE produtos SET subcategoria = 'Segurança' WHERE id = 72; -- Doleira RFID

-- Conforto
UPDATE produtos SET subcategoria = 'Conforto' WHERE id = 68; -- Travesseiro Cervical
UPDATE produtos SET subcategoria = 'Conforto' WHERE id = 73; -- Garrafa Termica 500ml

-- Acessórios
UPDATE produtos SET subcategoria = 'Acessórios' WHERE id = 66; -- Necessaire Organizadora
UPDATE produtos SET subcategoria = 'Acessórios' WHERE id = 74; -- Pochete Fanny Pack

-- Verificar resultado
SELECT categoria, subcategoria, count(*) FROM produtos GROUP BY categoria, subcategoria ORDER BY categoria, subcategoria;

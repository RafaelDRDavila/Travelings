-- ================================================================
--  SEED V2: Travelings - Lojas e Produtos com fotos corretas
--  Run via: POST http://localhost:5260/api/v1/migrate/seed-v2
-- ================================================================

-- Limpar dados existentes (respeitando FK)
DELETE FROM itensvendas;
DELETE FROM vendas;
DELETE FROM avaliacoes;
DELETE FROM favoritos;
DELETE FROM carrinho;
DELETE FROM produtos;
DELETE FROM lojas;

-- Reset sequences
ALTER SEQUENCE lojas_id_seq RESTART WITH 1;
ALTER SEQUENCE produtos_id_seq RESTART WITH 1;

-- ================================================================
--  LOJAS (6 lojas com nomes reais)
-- ================================================================

INSERT INTO lojas (nome, cnpj, descricao, logo, banner, email, telefone, endereco) VALUES
('Decathlon', '09609022000180',
 'Tudo para o esporte e aventura. Equipamentos para todos os niveis e modalidades.',
 'https://images.unsplash.com/photo-1571019613454-1cb2f99b2d8b?w=200&q=80',
 'https://images.unsplash.com/photo-1551632811-561732d1e306?w=1400&q=80',
 'contato@decathlon.com.br', '(11) 4003-7424', 'Av. Paulista 1100, Sao Paulo - SP'),

('Nautika Lazer', '50872833000120',
 'Referencia em equipamentos para camping, pesca e aventura ao ar livre desde 1993.',
 'https://images.unsplash.com/photo-1487730116645-74489c95b41b?w=200&q=80',
 'https://images.unsplash.com/photo-1504851149312-7a075b496cc7?w=1400&q=80',
 'sac@nautika.com.br', '(11) 2065-9800', 'Rua Industrial 300, Guarulhos - SP'),

('Havaianas', '61079117000105',
 'Nascida no Brasil, amada no mundo. Chinelos, sandalias e acessorios para o verao.',
 'https://images.unsplash.com/photo-1562183241-b937e95585b6?w=200&q=80',
 'https://images.unsplash.com/photo-1507525428034-b723cf961d3e?w=1400&q=80',
 'faleconosco@havaianas.com.br', '(11) 3797-0222', 'Rua Fernandes Moreira 1166, Sao Paulo - SP'),

('Samsonite', '02802609000108',
 'Lider mundial em malas e acessorios de viagem desde 1910.',
 'https://images.unsplash.com/photo-1581553680321-4fffae59fccd?w=200&q=80',
 'https://images.unsplash.com/photo-1565026057447-bc90a3dceb87?w=1400&q=80',
 'sac@samsonite.com.br', '(11) 3504-8888', 'Rua Pamplona 818, Sao Paulo - SP'),

('Quiksilver', '06057223000171',
 'Surf, neve e outdoor. Roupas e equipamentos para quem vive a natureza.',
 'https://images.unsplash.com/photo-1502680390548-bdbac40b3e1a?w=200&q=80',
 'https://images.unsplash.com/photo-1455729552457-5c322e38a211?w=1400&q=80',
 'atendimento@quiksilver.com.br', '(11) 3047-4700', 'Av. Nacoes Unidas 12901, Sao Paulo - SP'),

('Trilhas e Rumos', '06312878000107',
 'Especializada em trekking, montanhismo e viagens de aventura.',
 'https://images.unsplash.com/photo-1551632811-561732d1e306?w=200&q=80',
 'https://images.unsplash.com/photo-1464822759023-fed622ff2c3b?w=1400&q=80',
 'contato@trilhaserumos.com.br', '(11) 3062-8100', 'Rua Augusta 2200, Sao Paulo - SP');

-- ================================================================
--  PRODUTOS - PRAIA (12 produtos)
-- ================================================================

INSERT INTO produtos (nome, preco, descricao, estoque, categoria, subcategoria, imagem, idloja, ativo) VALUES

-- Havaianas
('Chinelo Havaianas Top', 39.90,
 'O classico chinelo brasileiro em borracha natural. Confortavel, duravel e perfeito para a praia.',
 50, 'Praia', 'Conforto',
 'https://images.unsplash.com/photo-1603487742131-4160ec999306?w=500&q=80',
 (SELECT id FROM lojas WHERE cnpj='61079117000105'), true),

('Protetor Solar FPS 70', 49.90,
 'Protecao solar resistente a agua por ate 4 horas. Formula leve e nao oleosa.',
 60, 'Praia', 'Proteção',
 'https://images.unsplash.com/photo-1620916566398-39f1143ab7be?w=500&q=80',
 (SELECT id FROM lojas WHERE cnpj='61079117000105'), true),

('Canga Toalha Microfibra', 69.90,
 'Canga 2 em 1: toalha de secagem rapida e canga de praia. Compacta e leve.',
 45, 'Praia', 'Conforto',
 'https://images.unsplash.com/photo-1519046904884-53103b34b206?w=500&q=80',
 (SELECT id FROM lojas WHERE cnpj='61079117000105'), true),

-- Decathlon
('Guarda-Sol Praia UV50', 149.90,
 'Guarda-sol com protecao UV50 e estrutura em aluminio. Inclui bolsa de transporte.',
 20, 'Praia', 'Proteção',
 'https://images.unsplash.com/photo-1473116763249-2faaef81ccda?w=500&q=80',
 1, true),

('Kit Snorkel Adulto', 89.90,
 'Mascara de silicone ajustavel com snorkel e valvula anti-retorno. Lente anti-embacante.',
 30, 'Praia', 'Diversão',
 'https://images.unsplash.com/photo-1544551763-46a013bb70d5?w=500&q=80',
 1, true),

('Cadeira de Praia Reclinavel', 119.90,
 'Cadeira com 5 posicoes de reclinacao, tecido impermeavel e estrutura em aluminio.',
 25, 'Praia', 'Conforto',
 'https://images.unsplash.com/photo-1531722569936-825d3dd91b15?w=500&q=80',
 1, true),

('Bolsa Termica 20L', 79.90,
 'Bolsa termica impermeavel com isolamento de ate 6 horas. Comporta 20 latas.',
 35, 'Praia', 'Hidratação',
 'https://images.unsplash.com/photo-1513116476489-7635e79feb27?w=500&q=80',
 1, true),

-- Quiksilver
('Boardshort Quiksilver', 249.90,
 'Bermuda de surf com tecido 4-way stretch e secagem ultra-rapida.',
 25, 'Praia', 'Acessórios',
 'https://images.unsplash.com/photo-1507525428034-b723cf961d3e?w=500&q=80',
 (SELECT id FROM lojas WHERE cnpj='06057223000171'), true),

('Bone Quiksilver Trucker', 129.90,
 'Bone com tela traseira respiravel e ajuste snapback. Protecao solar para o surf.',
 40, 'Praia', 'Acessórios',
 'https://images.unsplash.com/photo-1521369909029-2afed882baee?w=500&q=80',
 (SELECT id FROM lojas WHERE cnpj='06057223000171'), true),

('Oculos de Sol Polarizado', 299.90,
 'Lentes polarizadas que reduzem o reflexo na agua. Armacao leve e resistente.',
 30, 'Praia', 'Acessórios',
 'https://images.unsplash.com/photo-1572635196237-14b3f281503f?w=500&q=80',
 (SELECT id FROM lojas WHERE cnpj='06057223000171'), true),

('Prancha Bodyboard', 159.90,
 'Prancha de bodyboard em EPS com slick laminado e leash incluso.',
 15, 'Praia', 'Diversão',
 'https://images.unsplash.com/photo-1502933691298-84fc14542831?w=500&q=80',
 (SELECT id FROM lojas WHERE cnpj='06057223000171'), true),

('Boia Inflavel Grande', 59.90,
 'Boia inflavel com 120cm de diametro em vinil resistente. Design tropical.',
 40, 'Praia', 'Diversão',
 'https://images.unsplash.com/photo-1530092285049-1c42085fd395?w=500&q=80',
 (SELECT id FROM lojas WHERE cnpj='06057223000171'), true);

-- ================================================================
--  PRODUTOS - ACAMPAMENTO (12 produtos)
-- ================================================================

INSERT INTO produtos (nome, preco, descricao, estoque, categoria, subcategoria, imagem, idloja, ativo) VALUES

-- Nautika
('Barraca Camping 6 Pessoas', 599.90,
 'Barraca dupla camada com coluna d''agua 2000mm. Sobreteto impermeavel e varetas em fibra. Disponivel para aluguel em aventuras de curta duracao.',
 12, 'Acampamento', 'Abrigo',
 'https://images.unsplash.com/photo-1504851149312-7a075b496cc7?w=500&q=80',
 (SELECT id FROM lojas WHERE cnpj='50872833000120'), true),

('Saco de Dormir 10 Graus', 199.90,
 'Saco de dormir com temperatura de conforto de 10°C. Enchimento sintetico lavavel.',
 20, 'Acampamento', 'Conforto',
 'https://images.unsplash.com/photo-1510312305653-8ed496efae75?w=500&q=80',
 (SELECT id FROM lojas WHERE cnpj='50872833000120'), true),

('Lanterna LED 1000 Lumens', 89.90,
 'Lanterna tatica recarregavel USB-C com zoom ajustavel. Resistente a agua IPX4.',
 40, 'Acampamento', 'Iluminação',
 'https://images.unsplash.com/photo-1495954484750-af469f2f9be5?w=500&q=80',
 (SELECT id FROM lojas WHERE cnpj='50872833000120'), true),

('Fogareiro Portatil a Gas', 119.90,
 'Fogareiro com ignicao piezoeletrica automatica. Compativel com cartuchos de 227g.',
 20, 'Acampamento', 'Cozinha',
 'https://images.unsplash.com/photo-1596394516093-501ba68a0ba6?w=500&q=80',
 (SELECT id FROM lojas WHERE cnpj='50872833000120'), true),

('Colchonete Inflavel', 149.90,
 'Colchonete autoinflavel com espuma de alta densidade. Isolamento termico R3.',
 25, 'Acampamento', 'Conforto',
 'https://images.unsplash.com/photo-1504280390367-361c6d9f38f4?w=500&q=80',
 (SELECT id FROM lojas WHERE cnpj='50872833000120'), true),

-- Trilhas e Rumos
('Mochila Trekking 60L', 449.90,
 'Mochila cargueira com quadro ajustavel e capa de chuva integrada.',
 15, 'Acampamento', 'Equipamento',
 'https://images.unsplash.com/photo-1622260614153-03223fb72052?w=500&q=80',
 (SELECT id FROM lojas WHERE cnpj='06312878000107'), true),

('Kit Panelas Camping 5 Pecas', 129.90,
 'Conjunto com 2 panelas, frigideira, tampa multiuso e pegador dobravel antiaderente.',
 30, 'Acampamento', 'Cozinha',
 'https://images.unsplash.com/photo-1556909114-f6e7ad7d3136?w=500&q=80',
 (SELECT id FROM lojas WHERE cnpj='06312878000107'), true),

('Canivete Multiuso 12 Funcoes', 79.90,
 'Canivete inox com faca, serra, abridor, chave phillips, tesoura e mais 7 ferramentas.',
 50, 'Acampamento', 'Equipamento',
 'https://images.unsplash.com/photo-1550355291-bbee04a92027?w=500&q=80',
 (SELECT id FROM lojas WHERE cnpj='06312878000107'), true),

('Rede com Mosquiteiro', 99.90,
 'Rede de nylon ripstop com mosquiteiro integrado. Suporta 150kg, pesa 600g.',
 20, 'Acampamento', 'Conforto',
 'https://images.unsplash.com/photo-1520250497591-112f2f40a3f4?w=500&q=80',
 (SELECT id FROM lojas WHERE cnpj='06312878000107'), true),

-- Decathlon
('Cadeira Dobravel Camping', 99.90,
 'Cadeira com porta-copos e bolsa de transporte. Estrutura em aco, suporta 110kg.',
 30, 'Acampamento', 'Conforto',
 'https://images.unsplash.com/photo-1525811902-f2342640856e?w=500&q=80',
 1, true),

('Lampiao Solar Recarregavel', 69.90,
 'Lampiao com painel solar e USB. 3 modos de luz com ate 30 horas de autonomia.',
 35, 'Acampamento', 'Iluminação',
 'https://images.unsplash.com/photo-1563299796-17596ed6b017?w=500&q=80',
 1, true),

('Garrafa Termica 1 Litro', 89.90,
 'Aco inoxidavel com dupla parede a vacuo. Gelada 24h ou quente 12h.',
 40, 'Acampamento', 'Cozinha',
 'https://images.unsplash.com/photo-1602143407151-7111542de6e8?w=500&q=80',
 1, true);

-- ================================================================
--  PRODUTOS - TURISMO (12 produtos)
-- ================================================================

INSERT INTO produtos (nome, preco, descricao, estoque, categoria, subcategoria, imagem, idloja, ativo) VALUES

-- Samsonite
('Mala Spinner 68cm', 899.90,
 'Mala media com 4 rodas 360 graus, cadeado TSA integrado e divisorias organizadoras.',
 15, 'Turismo', 'Organização',
 'https://images.unsplash.com/photo-1565026057447-bc90a3dceb87?w=500&q=80',
 (SELECT id FROM lojas WHERE cnpj='02802609000108'), true),

('Mala de Bordo 55cm', 649.90,
 'Mala de mao aprovada por companhias aereas. Policarbonato ultra-leve.',
 20, 'Turismo', 'Organização',
 'https://images.unsplash.com/photo-1553062407-98eeb64c6a62?w=500&q=80',
 (SELECT id FROM lojas WHERE cnpj='02802609000108'), true),

('Mochila Anti-Furto', 349.90,
 'Abertura traseira anti-furto, porta USB externa e compartimento para notebook 15 pol.',
 25, 'Turismo', 'Segurança',
 'https://images.unsplash.com/photo-1553062407-98eeb64c6a62?w=500&q=80',
 (SELECT id FROM lojas WHERE cnpj='02802609000108'), true),

('Necessaire Organizadora', 149.90,
 'Necessaire com divisorias, gancho para banheiro e material impermeavel.',
 40, 'Turismo', 'Organização',
 'https://images.unsplash.com/photo-1581553680321-4fffae59fccd?w=500&q=80',
 (SELECT id FROM lojas WHERE cnpj='02802609000108'), true),

-- Decathlon
('Adaptador Universal de Tomada', 59.90,
 'Compativel com 150+ paises. 2 portas USB e 1 USB-C com protecao contra surtos.',
 60, 'Turismo', 'Eletrônicos',
 'https://images.unsplash.com/photo-1505740420928-5e560c06d30e?w=500&q=80',
 1, true),

('Travesseiro Cervical Memory Foam', 89.90,
 'Espuma viscoelastica com capa lavavel. Inclui bolsa compacta de transporte.',
 40, 'Turismo', 'Conforto',
 'https://images.unsplash.com/photo-1584100936595-c0654b55a2e2?w=500&q=80',
 1, true),

('Organizador de Malas 6 Pecas', 79.90,
 'Kit com 6 sacos em tamanhos variados para separar roupas, sapatos e acessorios.',
 35, 'Turismo', 'Organização',
 'https://images.unsplash.com/photo-1581553680321-4fffae59fccd?w=500&q=80',
 1, true),

('Cadeado TSA 3 Digitos', 39.90,
 'Cadeado aprovado TSA com combinacao de 3 digitos e indicador de inspecao.',
 55, 'Turismo', 'Segurança',
 'https://images.unsplash.com/photo-1614164185128-e4ec99c436d7?w=500&q=80',
 1, true),

-- Trilhas e Rumos
('Rastreador Bluetooth Bagagem', 129.90,
 'Rastreador para malas com bateria longa duracao. Localize pelo app em tempo real.',
 25, 'Turismo', 'Eletrônicos',
 'https://images.unsplash.com/photo-1585771724684-38269d6639fd?w=500&q=80',
 (SELECT id FROM lojas WHERE cnpj='06312878000107'), true),

('Doleira RFID Anti-Clonagem', 49.90,
 'Porta-documentos discreto para usar sob a roupa com bloqueio RFID.',
 55, 'Turismo', 'Segurança',
 'https://images.unsplash.com/photo-1488646953014-85cb44e25828?w=500&q=80',
 (SELECT id FROM lojas WHERE cnpj='06312878000107'), true),

('Garrafa Termica Viagem 500ml', 59.90,
 'Aco inox compacto com tampa anti-vazamento. Ideal para passeios e voos longos.',
 45, 'Turismo', 'Conforto',
 'https://images.unsplash.com/photo-1602143407151-7111542de6e8?w=500&q=80',
 (SELECT id FROM lojas WHERE cnpj='06312878000107'), true),

-- Quiksilver
('Pochete Esportiva', 89.90,
 'Pochete com compartimento principal e bolso frontal. Alca ajustavel e tecido resistente a agua.',
 35, 'Turismo', 'Acessórios',
 'https://images.unsplash.com/photo-1548036328-c9fa89d128fa?w=500&q=80',
 (SELECT id FROM lojas WHERE cnpj='06057223000171'), true);

-- ================================================================
--  VERIFICATION
-- ================================================================
SELECT '=== LOJAS ===' AS info;
SELECT id, nome FROM lojas ORDER BY id;

SELECT '=== PRODUTOS POR CATEGORIA ===' AS info;
SELECT categoria, count(*) AS total FROM produtos WHERE ativo = true GROUP BY categoria ORDER BY categoria;

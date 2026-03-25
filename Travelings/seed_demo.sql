-- ================================================================
--  SEED DEMO: Travelings - Lojas e Produtos
--  Run with: PGPASSWORD=Admin psql -h localhost -U postgres -d Web -f seed_demo.sql
-- ================================================================

-- Step 0: Expand column sizes if needed
ALTER TABLE produtos ALTER COLUMN nome TYPE varchar(100);
ALTER TABLE produtos ALTER COLUMN imagem TYPE varchar(1000);

-- Step 1: Clean existing seeded data (keep original rows)
DELETE FROM produtos WHERE id > 3;
DELETE FROM lojas WHERE id > 8;

-- ================================================================
--  LOJAS (6 stores)
-- ================================================================

-- 1. Decathlon (already exists id=8, UPDATE it)
UPDATE lojas SET
  descricao = 'Tudo para o esporte e aventura. Equipamentos para todos os niveis e modalidades esportivas.',
  logo = 'https://images.unsplash.com/photo-1517836357463-d25dfeac3438?w=200&q=80',
  banner = 'https://images.unsplash.com/photo-1551632811-561732d1e306?w=1400&q=80',
  email = 'contato@decathlon.com.br',
  telefone = '(11) 4003-7424',
  endereco = 'Av. Paulista 1100, Sao Paulo - SP'
WHERE id = 8;

-- 2. Nautika Lazer
INSERT INTO lojas (nome, cnpj, descricao, logo, banner, email, telefone, endereco)
VALUES (
  'Nautika Lazer',
  '50872833000120',
  'Referencia em equipamentos para camping, pesca e aventura ao ar livre desde 1993.',
  'https://images.unsplash.com/photo-1504280390367-361c6d9f38f4?w=200&q=80',
  'https://images.unsplash.com/photo-1504851149312-7a075b496cc7?w=1400&q=80',
  'sac@nautika.com.br',
  '(11) 2065-9800',
  'Rua Industrial 300, Guarulhos - SP'
);

-- 3. Havaianas (Alpargatas)
INSERT INTO lojas (nome, cnpj, descricao, logo, banner, email, telefone, endereco)
VALUES (
  'Havaianas',
  '61079117000105',
  'Nascida no Brasil, amada no mundo. Chinelos, sandalias e acessorios para o verao eterno.',
  'https://images.unsplash.com/photo-1603487742131-4160ec999306?w=200&q=80',
  'https://images.unsplash.com/photo-1507525428034-b723cf961d3e?w=1400&q=80',
  'faleconosco@havaianas.com.br',
  '(11) 3797-0222',
  'Rua Fernandes Moreira 1166, Sao Paulo - SP'
);

-- 4. Samsonite
INSERT INTO lojas (nome, cnpj, descricao, logo, banner, email, telefone, endereco)
VALUES (
  'Samsonite',
  '02802609000108',
  'Lider mundial em malas e acessorios de viagem desde 1910. Qualidade e durabilidade incomparaveis.',
  'https://images.unsplash.com/photo-1553062407-98eeb64c6a62?w=200&q=80',
  'https://images.unsplash.com/photo-1565026057447-bc90a3dceb87?w=1400&q=80',
  'sac@samsonite.com.br',
  '(11) 3504-8888',
  'Rua Pamplona 818, Sao Paulo - SP'
);

-- 5. Quiksilver (Boardriders)
INSERT INTO lojas (nome, cnpj, descricao, logo, banner, email, telefone, endereco)
VALUES (
  'Quiksilver',
  '06057223000171',
  'Surf, neve e outdoor. Roupas e equipamentos para quem vive a natureza intensamente.',
  'https://images.unsplash.com/photo-1502680390548-bdbac40b3e1a?w=200&q=80',
  'https://images.unsplash.com/photo-1455729552457-5c322e38a211?w=1400&q=80',
  'atendimento@quiksilver.com.br',
  '(11) 3047-4700',
  'Av. Nacoes Unidas 12901, Sao Paulo - SP'
);

-- 6. Trilhas e Rumos (Centauro)
INSERT INTO lojas (nome, cnpj, descricao, logo, banner, email, telefone, endereco)
VALUES (
  'Trilhas e Rumos',
  '06312878000107',
  'Especializada em trekking, montanhismo e viagens de aventura. Equipamentos de alta performance.',
  'https://images.unsplash.com/photo-1551632811-561732d1e306?w=200&q=80',
  'https://images.unsplash.com/photo-1464822759023-fed622ff2c3b?w=1400&q=80',
  'contato@trilhaserumos.com.br',
  '(11) 3062-8100',
  'Rua Augusta 2200, Sao Paulo - SP'
);

-- ================================================================
--  PRODUTOS - PRAIA (12 products)
-- ================================================================

INSERT INTO produtos (nome, preco, descricao, estoque, categoria, imagem, idloja, ativo) VALUES

-- Havaianas (praia)
('Chinelo Havaianas Top', 39.90,
 'O classico chinelo brasileiro em borracha natural. Confortavel, duravel e com estilo para qualquer ocasiao na praia.',
 50, 'Praia',
 'https://images.unsplash.com/photo-1603487742131-4160ec999306?w=500&q=80',
 (SELECT id FROM lojas WHERE cnpj='61079117000105'), true),

('Protetor Solar FPS 70 200ml', 49.90,
 'Protecao solar de alta performance resistente a agua por ate 4 horas. Formula leve e nao oleosa para peles sensiveis.',
 60, 'Praia',
 'https://images.unsplash.com/photo-1532274402911-5a369e4c4bb5?w=500&q=80',
 (SELECT id FROM lojas WHERE cnpj='61079117000105'), true),

('Canga Toalha Microfibra', 69.90,
 'Canga 2 em 1: toalha de secagem rapida e canga de praia. Compacta, leve e com estampas tropicais exclusivas.',
 45, 'Praia',
 'https://images.unsplash.com/photo-1506953823976-52e1fdc0149a?w=500&q=80',
 (SELECT id FROM lojas WHERE cnpj='61079117000105'), true),

-- Decathlon (praia)
('Guarda-Sol Praia UV40', 149.90,
 'Guarda-sol com protecao UV40 e estrutura em aluminio leve. Inclui bolsa de transporte e estaca para areia.',
 20, 'Praia',
 'https://images.unsplash.com/photo-1507525428034-b723cf961d3e?w=500&q=80',
 8, true),

('Kit Snorkel Adulto Completo', 89.90,
 'Kit com mascara de silicone ajustavel e snorkel com valvula anti-retorno. Lente anti-embacante de policarbonato.',
 30, 'Praia',
 'https://images.unsplash.com/photo-1544551763-77932f56a0ec?w=500&q=80',
 8, true),

('Prancha Bodyboard 42 Polegadas', 159.90,
 'Prancha de bodyboard em EPS com slick laminado e leash incluso. Ideal para iniciantes e surfistas intermediarios.',
 15, 'Praia',
 'https://images.unsplash.com/photo-1502680390548-bdbac40b3e1a?w=500&q=80',
 8, true),

('Cadeira de Praia Reclinavel 5 Posicoes', 119.90,
 'Cadeira com 5 posicoes de reclinacao, tecido impermeavel e estrutura reforçada em aluminio anodizado.',
 25, 'Praia',
 'https://images.unsplash.com/photo-1473116763249-2faaef81ccda?w=500&q=80',
 8, true),

('Bolsa Termica Praia 20L', 79.90,
 'Bolsa termica impermeavel com isolamento termico de ate 6 horas. Comporta ate 20 latas com espaco para gelo.',
 35, 'Praia',
 'https://images.unsplash.com/photo-1519046904884-53103b34b206?w=500&q=80',
 8, true),

-- Quiksilver (praia)
('Boardshort Quiksilver Highline', 249.90,
 'Bermuda de surf com tecido 4-way stretch e secagem ultra-rapida. Costura selada para maior durabilidade nas ondas.',
 25, 'Praia',
 'https://images.unsplash.com/photo-1455729552457-5c322e38a211?w=500&q=80',
 (SELECT id FROM lojas WHERE cnpj='06057223000171'), true),

('Bone Quiksilver Trucker', 129.90,
 'Bone com tela traseira respiravel e ajuste snapback. Aba curva para protecao solar durante o surf e a praia.',
 40, 'Praia',
 'https://images.unsplash.com/photo-1588850561407-ed78c334e67a?w=500&q=80',
 (SELECT id FROM lojas WHERE cnpj='06057223000171'), true),

('Oculos de Sol Quiksilver Polarizado', 299.90,
 'Oculos com lentes polarizadas que reduzem o reflexo do sol na agua. Armacao leve e resistente a impactos.',
 30, 'Praia',
 'https://images.unsplash.com/photo-1572635196237-14b3f281503f?w=500&q=80',
 (SELECT id FROM lojas WHERE cnpj='06057223000171'), true),

('Boia Inflavel Circular Grande', 59.90,
 'Boia inflavel com diametro de 120cm em vinil resistente. Valvula de seguranca dupla e design colorido tropical.',
 40, 'Praia',
 'https://images.unsplash.com/photo-1530092285049-1c42085fd395?w=500&q=80',
 (SELECT id FROM lojas WHERE cnpj='06057223000171'), true);


-- ================================================================
--  PRODUTOS - ACAMPAMENTO (12 products)
-- ================================================================

INSERT INTO produtos (nome, preco, descricao, estoque, categoria, imagem, idloja, ativo) VALUES

-- Nautika (acampamento)
('Barraca Nautika Cherokee 6 Pessoas', 599.90,
 'Barraca para 6 pessoas com coluna d''agua de 2000mm. Dupla camada com sobreteto impermeavel e varetas em fibra.',
 12, 'Acampamento',
 'https://images.unsplash.com/photo-1504851149312-7a075b496cc7?w=500&q=80',
 (SELECT id FROM lojas WHERE cnpj='50872833000120'), true),

('Saco de Dormir Confort 10 Graus', 199.90,
 'Saco de dormir com temperatura de conforto de 10 graus Celsius. Enchimento sintetico lavavel e ziper lateral.',
 20, 'Acampamento',
 'https://images.unsplash.com/photo-1537905569824-f89f14cceb68?w=500&q=80',
 (SELECT id FROM lojas WHERE cnpj='50872833000120'), true),

('Lanterna LED Recarregavel 1000lm', 89.90,
 'Lanterna tatica com 1000 lumens, zoom ajustavel e bateria recarregavel USB-C. Resistente a agua IPX4.',
 40, 'Acampamento',
 'https://images.unsplash.com/photo-1510312305653-8ed496efae75?w=500&q=80',
 (SELECT id FROM lojas WHERE cnpj='50872833000120'), true),

('Fogareiro Compacto a Gas Portatil', 119.90,
 'Fogareiro portatil com ignicao automatica piezoeletrica. Compativel com cartuchos padrao de 227g. Chama ajustavel.',
 20, 'Acampamento',
 'https://images.unsplash.com/photo-1517824806704-9040b037703b?w=500&q=80',
 (SELECT id FROM lojas WHERE cnpj='50872833000120'), true),

('Colchonete Inflavel Autoinflavel', 149.90,
 'Colchonete autoinflavel com espuma expandida de alta densidade. Isolamento termico R3 e valvula rapida.',
 25, 'Acampamento',
 'https://images.unsplash.com/photo-1478131143081-80f7f84ca84d?w=500&q=80',
 (SELECT id FROM lojas WHERE cnpj='50872833000120'), true),

-- Trilhas e Rumos (acampamento)
('Mochila Cargueira Trekking 60L', 449.90,
 'Mochila de trekking com quadro ajustavel, capa de chuva integrada e compartimento inferior para saco de dormir.',
 15, 'Acampamento',
 'https://images.unsplash.com/photo-1622260614153-03223fb72052?w=500&q=80',
 (SELECT id FROM lojas WHERE cnpj='06312878000107'), true),

('Kit Panelas Camping Aluminio 5 Pecas', 129.90,
 'Conjunto com 2 panelas, frigideira, tampa multiuso e pegador dobravel. Revestimento antiaderente para facil limpeza.',
 30, 'Acampamento',
 'https://images.unsplash.com/photo-1504280390367-361c6d9f38f4?w=500&q=80',
 (SELECT id FROM lojas WHERE cnpj='06312878000107'), true),

('Canivete Multiuso 12 Funcoes Inox', 79.90,
 'Canivete em aco inox com faca, serra, abridor de garrafas, chave phillips, tesoura e mais 7 ferramentas essenciais.',
 50, 'Acampamento',
 'https://images.unsplash.com/photo-1464822759023-fed622ff2c3b?w=500&q=80',
 (SELECT id FROM lojas WHERE cnpj='06312878000107'), true),

('Rede de Descanso Camping com Mosquiteiro', 99.90,
 'Rede de nylon ripstop com mosquiteiro integrado e tiras de fixacao. Suporta ate 150kg e pesa apenas 600g.',
 20, 'Acampamento',
 'https://images.unsplash.com/photo-1520250497591-112f2f40a3f4?w=500&q=80',
 (SELECT id FROM lojas WHERE cnpj='06312878000107'), true),

-- Decathlon (acampamento)
('Cadeira Dobravel Camping Compacta', 99.90,
 'Cadeira dobravel com porta-copos e bolsa de transporte inclusa. Estrutura em aco reforçado, suporta ate 110kg.',
 30, 'Acampamento',
 'https://images.unsplash.com/photo-1525811902-f2342640856e?w=500&q=80',
 8, true),

('Lampiao LED Solar Recarregavel', 69.90,
 'Lampiao com painel solar integrado e carregamento por USB. 3 modos de iluminacao com ate 30 horas de autonomia.',
 35, 'Acampamento',
 'https://images.unsplash.com/photo-1476514525535-07fb3b4ae5f1?w=500&q=80',
 8, true),

('Garrafa Termica Camping 1 Litro', 89.90,
 'Garrafa de aco inoxidavel com dupla parede a vacuo. Mantem bebida gelada por 24h ou quente por 12h. Tampa com dosador.',
 40, 'Acampamento',
 'https://images.unsplash.com/photo-1516035069371-29a1b244cc32?w=500&q=80',
 8, true);


-- ================================================================
--  PRODUTOS - TURISMO (12 products)
-- ================================================================

INSERT INTO produtos (nome, preco, descricao, estoque, categoria, imagem, idloja, ativo) VALUES

-- Samsonite (turismo)
('Mala de Viagem Samsonite Spinner 68cm', 899.90,
 'Mala media com 4 rodas giratorias 360 graus, cadeado TSA integrado e interior com divisorias organizadoras.',
 15, 'Turismo',
 'https://images.unsplash.com/photo-1553062407-98eeb64c6a62?w=500&q=80',
 (SELECT id FROM lojas WHERE cnpj='02802609000108'), true),

('Mala de Bordo Samsonite 55cm', 649.90,
 'Mala de mao aprovada por companhias aereas internacionais. Policarbonato ultra-leve e resistente a impactos.',
 20, 'Turismo',
 'https://images.unsplash.com/photo-1565026057447-bc90a3dceb87?w=500&q=80',
 (SELECT id FROM lojas WHERE cnpj='02802609000108'), true),

('Mochila Anti-Furto Samsonite', 349.90,
 'Mochila com abertura traseira anti-furto, porta USB externa e compartimento acolchoado para notebook ate 15 pol.',
 25, 'Turismo',
 'https://images.unsplash.com/photo-1553062407-98eeb64c6a62?w=500&q=80',
 (SELECT id FROM lojas WHERE cnpj='02802609000108'), true),

('Necessaire Organizadora Samsonite', 149.90,
 'Necessaire com divisorias internas, gancho para pendurar em banheiros e material impermeavel facil de limpar.',
 40, 'Turismo',
 'https://images.unsplash.com/photo-1581553680321-4fffae59fccd?w=500&q=80',
 (SELECT id FROM lojas WHERE cnpj='02802609000108'), true),

-- Decathlon (turismo)
('Adaptador Universal de Tomada', 59.90,
 'Adaptador compativel com mais de 150 paises. 2 portas USB e 1 USB-C com protecao contra surtos de energia.',
 60, 'Turismo',
 'https://images.unsplash.com/photo-1505740420928-5e560c06d30e?w=500&q=80',
 8, true),

('Travesseiro Cervical Memory Foam', 89.90,
 'Travesseiro de viagem em espuma viscoelastica com capa lavavel removivel. Inclui bolsa compacta de transporte.',
 40, 'Turismo',
 'https://images.unsplash.com/photo-1520250497591-112f2f40a3f4?w=500&q=80',
 8, true),

('Organizador de Malas 6 Pecas', 79.90,
 'Kit com 6 sacos organizadores em tamanhos variados. Separe roupas, sapatos e acessorios dentro da mala com facilidade.',
 35, 'Turismo',
 'https://images.unsplash.com/photo-1553062407-98eeb64c6a62?w=500&q=80',
 8, true),

('Cadeado TSA com Segredo 3 Digitos', 39.90,
 'Cadeado aprovado pela TSA com combinacao de 3 digitos. Indicador vermelho que mostra se foi inspecionado.',
 55, 'Turismo',
 'https://images.unsplash.com/photo-1565026057447-bc90a3dceb87?w=500&q=80',
 8, true),

-- Trilhas e Rumos (turismo)
('Tag Rastreador de Bagagem Bluetooth', 129.90,
 'Rastreador Bluetooth para malas com bateria de longa duracao. Localiza sua bagagem pelo app em tempo real global.',
 25, 'Turismo',
 'https://images.unsplash.com/photo-1565026057447-bc90a3dceb87?w=500&q=80',
 (SELECT id FROM lojas WHERE cnpj='06312878000107'), true),

('Doleira de Viagem com Bloqueio RFID', 49.90,
 'Porta-documentos discreto para usar sob a roupa com bloqueio RFID contra clonagem de cartoes e passaporte.',
 55, 'Turismo',
 'https://images.unsplash.com/photo-1488646953014-85cb44e25828?w=500&q=80',
 (SELECT id FROM lojas WHERE cnpj='06312878000107'), true),

('Garrafa Termica Viagem 500ml', 59.90,
 'Garrafa termica compacta em aco inox com tampa anti-vazamento. Ideal para passeios turisticos e voos longos.',
 45, 'Turismo',
 'https://images.unsplash.com/photo-1516035069371-29a1b244cc32?w=500&q=80',
 (SELECT id FROM lojas WHERE cnpj='06312878000107'), true),

('Pochete Fanny Pack Esportiva', 89.90,
 'Pochete com compartimento principal e bolso frontal com ziper. Alca ajustavel e tecido resistente a agua.',
 35, 'Turismo',
 'https://images.unsplash.com/photo-1553062407-98eeb64c6a62?w=500&q=80',
 (SELECT id FROM lojas WHERE cnpj='06057223000171'), true);


-- ================================================================
--  VERIFICATION
-- ================================================================
SELECT '=== LOJAS ===' AS info;
SELECT id, nome, cnpj FROM lojas ORDER BY id;

SELECT '=== PRODUTOS POR CATEGORIA ===' AS info;
SELECT categoria, count(*) AS total FROM produtos GROUP BY categoria ORDER BY categoria;

SELECT '=== TOTAL PRODUTOS ===' AS info;
SELECT count(*) AS total_produtos FROM produtos;

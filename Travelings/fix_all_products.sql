-- ============================================================
-- COMPLETE PRODUCT UPDATE: images + store assignments
-- ============================================================
-- Stores:
--   1 = Decathlon (multi-esporte: praia, camping, turismo)
--   2 = Nautika Lazer (camping, pesca, aventura)
--   3 = Havaianas (praia, moda praia)
--   4 = Samsonite (malas, acessorios viagem)
--   5 = Quiksilver (surf, praia, outdoor)
--   6 = Trilhas e Rumos (trekking, montanhismo, aventura)
-- ============================================================

-- ======================== PRAIA - VENDA ========================

-- ID 1: Chinelo Havaianas Top → Havaianas (loja 3)
UPDATE produtos SET
  imagem = 'https://images.unsplash.com/photo-1603487742131-4160ec999306?w=500&q=80',
  idloja = 3
WHERE id = 1;

-- ID 2: Protetor Solar FPS 70 → Havaianas (loja 3) - lifestyle praia
UPDATE produtos SET
  imagem = 'https://images.unsplash.com/photo-1620916566398-39f1143ab7be?w=500&q=80',
  idloja = 3
WHERE id = 2;

-- ID 3: Canga Toalha Microfibra → Havaianas (loja 3)
UPDATE produtos SET
  imagem = 'https://images.unsplash.com/photo-1600369672770-985fd30004eb?w=500&q=80',
  idloja = 3
WHERE id = 3;

-- ID 4: Guarda-Sol Praia UV50 → Decathlon (loja 1)
UPDATE produtos SET
  imagem = 'https://images.unsplash.com/photo-1531256456869-ce942a665e80?w=500&q=80',
  idloja = 1
WHERE id = 4;

-- ID 5: Kit Snorkel Adulto → Decathlon (loja 1)
UPDATE produtos SET
  imagem = 'https://images.unsplash.com/photo-1544551763-46a013bb70d5?w=500&q=80',
  idloja = 1
WHERE id = 5;

-- ID 6: Cadeira de Praia Reclinavel → Decathlon (loja 1)
UPDATE produtos SET
  imagem = 'https://images.unsplash.com/photo-1531722569936-825d3dd91b15?w=500&q=80',
  idloja = 1
WHERE id = 6;

-- ID 7: Bolsa Termica 20L → Decathlon (loja 1)
UPDATE produtos SET
  imagem = 'https://images.unsplash.com/photo-1513116476489-7635e79feb27?w=500&q=80',
  idloja = 1
WHERE id = 7;

-- ID 8: Boardshort Quiksilver → Quiksilver (loja 5)
UPDATE produtos SET
  imagem = 'https://images.unsplash.com/photo-1565299585323-38d6b0865b47?w=500&q=80',
  idloja = 5
WHERE id = 8;

-- ID 9: Bone Quiksilver Trucker → Quiksilver (loja 5)
UPDATE produtos SET
  imagem = 'https://images.unsplash.com/photo-1521369909029-2afed882baee?w=500&q=80',
  idloja = 5
WHERE id = 9;

-- ID 10: Oculos de Sol Polarizado → Quiksilver (loja 5)
UPDATE produtos SET
  imagem = 'https://images.unsplash.com/photo-1572635196237-14b3f281503f?w=500&q=80',
  idloja = 5
WHERE id = 10;

-- ID 11: Prancha Bodyboard → Quiksilver (loja 5)
UPDATE produtos SET
  imagem = 'https://images.unsplash.com/photo-1502933691298-84fc14542831?w=500&q=80',
  idloja = 5
WHERE id = 11;

-- ID 12: Boia Inflavel Grande → Decathlon (loja 1)
UPDATE produtos SET
  imagem = 'https://images.unsplash.com/photo-1560089000-7433a4ebbd64?w=500&q=80',
  idloja = 1
WHERE id = 12;

-- ======================== ACAMPAMENTO - VENDA ========================

-- ID 13: Barraca Camping 6 Pessoas → Nautika (loja 2)
UPDATE produtos SET
  imagem = 'https://images.unsplash.com/photo-1478131143081-80f7f84ca84d?w=500&q=80',
  idloja = 2
WHERE id = 13;

-- ID 14: Saco de Dormir 10 Graus → Nautika (loja 2)
UPDATE produtos SET
  imagem = 'https://images.unsplash.com/photo-1510312305653-8ed496efae75?w=500&q=80',
  idloja = 2
WHERE id = 14;

-- ID 15: Lanterna LED 1000 Lumens → Nautika (loja 2)
UPDATE produtos SET
  imagem = 'https://images.unsplash.com/photo-1495954484750-af469f2f9be5?w=500&q=80',
  idloja = 2
WHERE id = 15;

-- ID 16: Fogareiro Portatil a Gas → Nautika (loja 2)
UPDATE produtos SET
  imagem = 'https://images.unsplash.com/photo-1596394516093-501ba68a0ba6?w=500&q=80',
  idloja = 2
WHERE id = 16;

-- ID 17: Colchonete Inflavel → Decathlon (loja 1)
UPDATE produtos SET
  imagem = 'https://images.unsplash.com/photo-1504280390367-361c6d9f38f4?w=500&q=80',
  idloja = 1
WHERE id = 17;

-- ID 18: Mochila Trekking 60L → Trilhas e Rumos (loja 6)
UPDATE produtos SET
  imagem = 'https://images.unsplash.com/photo-1622260614153-03223fb72052?w=500&q=80',
  idloja = 6
WHERE id = 18;

-- ID 19: Kit Panelas Camping 5 Pecas → Nautika (loja 2)
UPDATE produtos SET
  imagem = 'https://images.unsplash.com/photo-1556909114-f6e7ad7d3136?w=500&q=80',
  idloja = 2
WHERE id = 19;

-- ID 20: Canivete Multiuso 12 Funcoes → Trilhas e Rumos (loja 6)
UPDATE produtos SET
  imagem = 'https://images.unsplash.com/photo-1550355291-bbee04a92027?w=500&q=80',
  idloja = 6
WHERE id = 20;

-- ID 21: Rede com Mosquiteiro → Trilhas e Rumos (loja 6)
UPDATE produtos SET
  imagem = 'https://images.unsplash.com/photo-1474552226712-ac0f0961a954?w=500&q=80',
  idloja = 6
WHERE id = 21;

-- ID 22: Cadeira Dobravel Camping → Decathlon (loja 1)
UPDATE produtos SET
  imagem = 'https://images.unsplash.com/photo-1525811902-f2342640856e?w=500&q=80',
  idloja = 1
WHERE id = 22;

-- ID 23: Lampiao Solar Recarregavel → Decathlon (loja 1)
UPDATE produtos SET
  imagem = 'https://images.unsplash.com/photo-1563299796-17596ed6b017?w=500&q=80',
  idloja = 1
WHERE id = 23;

-- ID 24: Garrafa Termica 1 Litro → Decathlon (loja 1)
UPDATE produtos SET
  imagem = 'https://images.unsplash.com/photo-1602143407151-7111542de6e8?w=500&q=80',
  idloja = 1
WHERE id = 24;

-- ======================== TURISMO - VENDA ========================

-- ID 25: Mala Spinner 68cm → Samsonite (loja 4)
UPDATE produtos SET
  imagem = 'https://images.unsplash.com/photo-1565026057447-bc90a3dceb87?w=500&q=80',
  idloja = 4
WHERE id = 25;

-- ID 26: Mala de Bordo 55cm → Samsonite (loja 4)
UPDATE produtos SET
  imagem = 'https://images.unsplash.com/photo-1553062407-98eeb64c6a62?w=500&q=80',
  idloja = 4
WHERE id = 26;

-- ID 27: Mochila Anti-Furto → Trilhas e Rumos (loja 6)
UPDATE produtos SET
  imagem = 'https://images.unsplash.com/photo-1553062407-98eeb64c6a25?w=500&q=80',
  idloja = 6
WHERE id = 27;

-- ID 28: Necessaire Organizadora → Samsonite (loja 4)
UPDATE produtos SET
  imagem = 'https://images.unsplash.com/photo-1581553680321-4fffae59fccd?w=500&q=80',
  idloja = 4
WHERE id = 28;

-- ID 29: Adaptador Universal de Tomada → Decathlon (loja 1)
UPDATE produtos SET
  imagem = 'https://images.unsplash.com/photo-1621600411688-4571f93b56e0?w=500&q=80',
  idloja = 1
WHERE id = 29;

-- ID 30: Travesseiro Cervical Memory Foam → Samsonite (loja 4)
UPDATE produtos SET
  imagem = 'https://images.unsplash.com/photo-1584100936595-c0654b55a2e2?w=500&q=80',
  idloja = 4
WHERE id = 30;

-- ID 31: Organizador de Malas 6 Pecas → Samsonite (loja 4)
UPDATE produtos SET
  imagem = 'https://images.unsplash.com/photo-1581553680321-4fffae59fccd?w=500&q=80',
  idloja = 4
WHERE id = 31;

-- ID 32: Cadeado TSA 3 Digitos → Samsonite (loja 4)
UPDATE produtos SET
  imagem = 'https://images.unsplash.com/photo-1614164185128-e4ec99c436d7?w=500&q=80',
  idloja = 4
WHERE id = 32;

-- ID 33: Rastreador Bluetooth Bagagem → Trilhas e Rumos (loja 6)
UPDATE produtos SET
  imagem = 'https://images.unsplash.com/photo-1585771724684-38269d6639fd?w=500&q=80',
  idloja = 6
WHERE id = 33;

-- ID 34: Doleira RFID Anti-Clonagem → Trilhas e Rumos (loja 6)
UPDATE produtos SET
  imagem = 'https://images.unsplash.com/photo-1548036328-c9fa89d128fa?w=500&q=80',
  idloja = 6
WHERE id = 34;

-- ID 35: Garrafa Termica Viagem 500ml → Decathlon (loja 1)
UPDATE produtos SET
  imagem = 'https://images.unsplash.com/photo-1602143407151-7111542de6e8?w=500&q=80',
  idloja = 1
WHERE id = 35;

-- ID 36: Pochete Esportiva → Quiksilver (loja 5)
UPDATE produtos SET
  imagem = 'https://images.unsplash.com/photo-1556306535-0f09a537f0a3?w=500&q=80',
  idloja = 5
WHERE id = 36;

-- ======================== PRAIA - ALUGUEL ========================

-- ID 37: Guarda-Sol Premium UV50+ → Decathlon (loja 1) - equipamento praia
UPDATE produtos SET
  imagem = 'https://images.unsplash.com/photo-1531256456869-ce942a665e80?w=500&q=80',
  idloja = 1
WHERE id = 37;

-- ID 38: Prancha de Surf Iniciante → Quiksilver (loja 5) - surf
UPDATE produtos SET
  imagem = 'https://images.unsplash.com/photo-1502680390548-bdbac40b3e1a?w=500&q=80',
  idloja = 5
WHERE id = 38;

-- ======================== ACAMPAMENTO - ALUGUEL ========================

-- ID 39: Saco de Dormir Termico -5C → Nautika (loja 2) - camping
UPDATE produtos SET
  imagem = 'https://images.unsplash.com/photo-1510312305653-8ed496efae75?w=500&q=80',
  idloja = 2
WHERE id = 39;

-- ID 40: Lanterna Frontal Recarregavel → Trilhas e Rumos (loja 6) - trekking
UPDATE produtos SET
  imagem = 'https://images.unsplash.com/photo-1495954484750-af469f2f9be5?w=500&q=80',
  idloja = 6
WHERE id = 40;

-- ======================== TURISMO - ALUGUEL ========================

-- ID 41: Camera GoPro Hero 12 → Decathlon (loja 1) - eletronicos esportivos
UPDATE produtos SET
  imagem = 'https://images.unsplash.com/photo-1526170375885-4d8ecf77b99f?w=500&q=80',
  idloja = 1
WHERE id = 41;

-- ID 42: Binoculos Compactos 10x42 → Trilhas e Rumos (loja 6) - aventura
UPDATE produtos SET
  imagem = 'https://images.unsplash.com/photo-1509281373149-e957c6296406?w=500&q=80',
  idloja = 6
WHERE id = 42;

-- ======================== PRAIA - AMBOS ========================

-- ID 43: Kit Snorkel Profissional → Decathlon (loja 1) - equipamento mergulho
UPDATE produtos SET
  imagem = 'https://images.unsplash.com/photo-1544551763-46a013bb70d5?w=500&q=80',
  idloja = 1
WHERE id = 43;

-- ID 44: Cadeira de Praia Reclinavel → Decathlon (loja 1) - equipamento praia
UPDATE produtos SET
  imagem = 'https://images.unsplash.com/photo-1531722569936-825d3dd91b15?w=500&q=80',
  idloja = 1
WHERE id = 44;

-- ======================== ACAMPAMENTO - AMBOS ========================

-- ID 45: Barraca Camping 4 Pessoas → Nautika (loja 2) - camping
UPDATE produtos SET
  imagem = 'https://images.unsplash.com/photo-1504280390367-361c6d9f38f4?w=500&q=80',
  idloja = 2
WHERE id = 45;

-- ID 46: Fogareiro Portatil a Gas → Nautika (loja 2) - camping
UPDATE produtos SET
  imagem = 'https://images.unsplash.com/photo-1596394516093-501ba68a0ba6?w=500&q=80',
  idloja = 2
WHERE id = 46;

-- ======================== TURISMO - AMBOS ========================

-- ID 47: Mala de Viagem Rigida 28pol → Samsonite (loja 4) - malas
UPDATE produtos SET
  imagem = 'https://images.unsplash.com/photo-1565026057447-bc90a3dceb87?w=500&q=80',
  idloja = 4
WHERE id = 47;

-- ID 48: Power Bank Solar 20000mAh → Decathlon (loja 1) - eletronicos outdoor
UPDATE produtos SET
  imagem = 'https://images.unsplash.com/photo-1585338107529-13afc5f02586?w=500&q=80',
  idloja = 1
WHERE id = 48;

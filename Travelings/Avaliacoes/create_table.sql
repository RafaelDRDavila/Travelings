CREATE TABLE IF NOT EXISTS avaliacoes (
    id SERIAL PRIMARY KEY,
    idproduto INTEGER NOT NULL,
    idcliente INTEGER NOT NULL,
    nota INTEGER NOT NULL CHECK (nota >= 0 AND nota <= 5),
    texto TEXT NOT NULL,
    midias TEXT,
    nomecliente TEXT,
    fotocliente TEXT,
    datacriacao TIMESTAMP NOT NULL DEFAULT NOW(),
    UNIQUE(idcliente, idproduto)
);

CREATE INDEX IF NOT EXISTS idx_avaliacoes_idproduto ON avaliacoes(idproduto);

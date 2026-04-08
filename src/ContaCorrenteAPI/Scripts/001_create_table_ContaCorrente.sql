CREATE TABLE IF NOT EXISTS contacorrente (
    idcontacorrente TEXT PRIMARY KEY,
    numero INTEGER NOT NULL UNIQUE,
    nome TEXT NOT NULL,
    cpf TEXT NOT NULL UNIQUE,
    ativo INTEGER NOT NULL DEFAULT 0,
    senha TEXT NOT NULL,
    salt TEXT NOT NULL,
    CHECK (ativo IN (0,1))
);

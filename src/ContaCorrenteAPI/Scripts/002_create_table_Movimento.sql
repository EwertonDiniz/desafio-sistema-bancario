CREATE TABLE IF NOT EXISTS movimento (
    idmovimento TEXT PRIMARY KEY,
    idcontacorrente TEXT NOT NULL,
    datamovimento TEXT NOT NULL,
    valor REAL NOT NULL,
    tipomovimento TEXT NOT NULL,
    idempotentkey TEXT NOT NULL,
    CHECK (tipomovimento IN ('C','D')),
    FOREIGN KEY(idcontacorrente) REFERENCES contacorrente(idcontacorrente)
);

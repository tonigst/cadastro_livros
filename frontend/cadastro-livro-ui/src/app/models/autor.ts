export class Autor {
    constructor(codAu: number = 0, nome: string = '') {
        this.codAu = codAu;
        this.nome = nome;
    }

    public codAu: number;
    public nome: string;
}
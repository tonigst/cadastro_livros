export class Preco {
    constructor(codP: number = 0, codFC: number = 0, formaCompra: string = '', valor: number = 0 ) {
        this.codP = codP;
        this.codFC = codFC;
        this.formaCompra = formaCompra;
        this.valor = valor;
    }

    public codP: number;
    public codFC: number;
    public formaCompra: string;
    public valor: number;
}
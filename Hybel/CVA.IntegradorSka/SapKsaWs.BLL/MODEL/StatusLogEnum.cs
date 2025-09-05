namespace SapKsaWs.BLL.MODEL
{
    public enum StatusLogEnum
    {
        AguardandoProcessamento = 0,
        EmProcessamento = 1,
        ImportadoComSucesso = 2,
        QuantidadeEstoqueInsuficiente = 3,
        ItensComPosicoesInvalidas = 4,
        SemConsumoMaterialNaPosicao = 5,
        GerarAcabado = 6,
        QuantidadeZero = 7,
        PosicaoOPFechada = 8,
        UsuarioNaoCadastrado = 9,
        SemEntradaAcabado = 10,
        VerificarObservacao = 99
    }
}

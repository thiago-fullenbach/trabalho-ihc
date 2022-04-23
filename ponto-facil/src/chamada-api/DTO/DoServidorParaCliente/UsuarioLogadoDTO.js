export default class SessaoDTO {
    get int_id() {
        return this.Id;
    }
    set int_id(value) {
        this.Id = value;
    }

    get string_nome() {
        return this.Nome;
    }
    set string_nome(value) {
        this.Nome = value;
    }

    get string_cpf() {
        return this.CPF;
    }
    set string_cpf(value) {
        this.CPF = value;
    }

    get date_data_nascimento() {
        return this.Data_nascimento;
    }
    set date_data_nascimento(value) {
        this.Data_nascimento = value;
    }
    
    get int_horas_diarias() {
        return this.Horas_diarias;
    }
    set int_horas_diarias(value) {
        this.Horas_diarias = value;
    }

    get string_login() {
        return this.Login;
    }
    set string_login(value) {
        this.Login = value;
    }

    get UsuarioRecursoDTO_navegacaoRecurso() {
        return this.NavegacaoRecurso;
    }
    set UsuarioRecursoDTO_navegacaoRecurso(value) {
        this.NavegacaoRecurso = value;
    }
}
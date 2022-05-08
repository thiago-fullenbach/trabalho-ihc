export default class CadUsuarioCadastreSeDTO {
    get string_nome() {
        return this.nome;
    }
    set string_nome(value) {
        this.nome = value;
    }

    get string_cpf() {
        return this.cpf;
    }
    set string_cpf(value) {
        this.cpf = value;
    }

    get date_data_nascimento() {
        return this.data_nascimento;
    }
    set date_data_nascimento(value) {
        this.data_nascimento = value;
    }
    
    get int_horas_diarias() {
        return this.horas_diarias;
    }
    set int_horas_diarias(value) {
        this.horas_diarias = value;
    }

    get string_login() {
        return this.login;
    }
    set string_login(value) {
        this.login = value;
    }

    get string_senha() {
        return this.senha;
    }
    set string_senha(value) {
        this.senha = value;
    }
}

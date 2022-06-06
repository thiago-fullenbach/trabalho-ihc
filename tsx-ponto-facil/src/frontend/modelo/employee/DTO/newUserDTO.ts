import UserAccess from "../userAccess"

export default class NewUserDTO {
    
    nome = ``
    cpf = ``
    data_nascimento = new Date()
    horas_diarias = 0
    login = ``
    nova_senha = ``
    acessos: UserAccess[] = []
}
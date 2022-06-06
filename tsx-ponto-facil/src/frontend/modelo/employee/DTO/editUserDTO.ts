import UserAccess from "../userAccess"

export default class EditUserDTO {
    
    id = 0
    nome = ``
    cpf = ``
    data_nascimento = new Date()
    horas_diarias = 0
    login = ``
    nova_senha: string | null = null
    acessos: UserAccess[] = []
}
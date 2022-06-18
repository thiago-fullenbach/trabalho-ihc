import UserAccess from "../userAccess"

export default class DetailedUserDTO {
    
    id = 0
    nome = ``
    cpf = ``
    data_nascimento = new Date()
    horas_diarias = 0
    login = ``
    eh_admin_root = false
    acessos: UserAccess[] = []
}
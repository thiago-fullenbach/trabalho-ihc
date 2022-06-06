import UserAccess from "./userAccess"

export default class User {
    
    id = 0
    nome = ``
    cpf = ``
    data_nascimento: Date | null = null
    horas_diarias: number | null = 0
    login = ``
    nova_senha = ``
    confirmar_senha = ``
    acessos: UserAccess[] = []
}
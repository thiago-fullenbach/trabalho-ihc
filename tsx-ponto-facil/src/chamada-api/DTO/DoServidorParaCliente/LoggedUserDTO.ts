import LoggedUserAccessDTO from "./LoggedUserAccessDTO"

export default class LoggedUserDTO {
    Id: number = 0
    Nome: string = ``
    CPF: string = ``
    Data_nascimento: Date = new Date()
    Horas_diarias: number = 0
    Login: string = ``
    AccessList: LoggedUserAccessDTO[] = []
}
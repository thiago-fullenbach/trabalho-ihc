import IReturnedXMessagesAdapter from "../../Adapter/IReturnedXMessagesAdapter";
import ReturnedXMessagesSuccess from "../ReturnedXMessagesSuccess";

export default class ReturnedXMessagesSuccessAdapter<T> implements IReturnedXMessagesAdapter<T> {
    adaptee: ReturnedXMessagesSuccess<T>
    constructor(adaptee: ReturnedXMessagesSuccess<T>) {
        this.adaptee = adaptee
    }
    getReturned(): T | null {
        return this.adaptee.devolvido || null
    }
    setReturned(returned: T): void {
        this.adaptee.devolvido = returned
    }
    getMessages(): string[] {
        return this.adaptee.mensagens
    }
    setMessages(messages: string[]): void {
        this.adaptee.mensagens = messages
    }
}
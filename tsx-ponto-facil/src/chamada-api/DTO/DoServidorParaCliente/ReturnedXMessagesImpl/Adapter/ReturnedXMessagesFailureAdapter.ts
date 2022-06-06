import IReturnedXMessagesAdapter from "../../Adapter/IReturnedXMessagesAdapter";
import ReturnedXMessagesFailure from "../ReturnedXMessagesFailure";

export default class ReturnedXMessagesFailureAdapter<T> implements IReturnedXMessagesAdapter<T> {
    adaptee: ReturnedXMessagesFailure<T>
    constructor(adaptee: ReturnedXMessagesFailure<T>) {
        this.adaptee = adaptee
    }
    getReturned(): T | null {
        return this.adaptee.Devolvido || null
    }
    setReturned(returned: T): void {
        this.adaptee.Devolvido = returned
    }
    getMessages(): string[] {
        return this.adaptee.Mensagens
    }
    setMessages(messages: string[]): void {
        this.adaptee.Mensagens = messages
    }
}
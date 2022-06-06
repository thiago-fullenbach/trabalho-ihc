export default interface IReturnedXMessagesAdapter<T> {
    getReturned() : T | null
    setReturned(returned: T): void
    getMessages() : string[]
    setMessages(messages: string[]): void 
}
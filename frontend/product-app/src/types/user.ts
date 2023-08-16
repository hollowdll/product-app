export interface UserData {
    userId: string,
    username: string,
    roles: Array<UserRole>,
}

export interface UserRole {
    name: string,
}
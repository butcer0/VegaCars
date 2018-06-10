import { make, model } from "./carOptions";


export class Car {
    make: make;
    model: model;
    isRegistered: boolean = false;
    features: Feature[] = [];
    contactInfo: Contact = new Contact();
}

export class Feature {
    id: number = 0;
    name: string = '';
}

export class Contact {
    name: string = '';
    phone: string = '';
    email: string = '';
}
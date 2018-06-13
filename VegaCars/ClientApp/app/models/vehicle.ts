import { KeyValuePair } from "./keyvaluepair";
import { Contact } from "./Contact";




export interface Vehicle {
    id: number;
    model: KeyValuePair;
    make: KeyValuePair;
    isRegistered: boolean,
    features: KeyValuePair[],
    contact: Contact;
    lastUpdated: string;
}


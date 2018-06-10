import { Component, Inject } from '@angular/core';
import { Car, Feature, Contact } from '../../Models/Car';
import { make, model } from '../../Models/carOptions';
import { Http } from '@angular/http';

@Component({
    selector: 'app-addvehicle',
    templateUrl: './addvehicle.component.html',
    styleUrls: ['./addvehicle.component.css']
})

export class AddvehicleComponent {
    car: Car = new Car();
    addCarButtonActive: boolean = false;
    makes: make[] = [];

    constructor(http: Http, @Inject('BASE_URL') baseUrl: string) {
        http.get(baseUrl + 'api/makes').subscribe(result => {
            this.makes = result.json() as make[];
        }, error => console.error(error));
    }


    makeSelected(make: make) {
        this.car.make = make;
    }

    modelSelected(model: model) {
        this.car.model = model;
    }

    changeRegistration() {
        this.checkButtonAddActive();
    }

    changeContactInfo() {
        this.checkButtonAddActive();
    }

    checkButtonAddActive() {
        if (this.car.make && this.car.model && this.car.contactInfo && this.car.contactInfo.email && this.car.contactInfo.name && this.car.contactInfo.phone) {
            this.addCarButtonActive = true;
        } else {
            this.addCarButtonActive = false;
        }
    }

}
import { Component, OnInit } from '@angular/core';
import { VehicleService } from '../../services/vehicle.service';
import { ActivatedRoute } from '@angular/router/';
import { ToastyService, ToastyConfig, ToastOptions, ToastData } from 'ng2-toasty';

@Component({
  selector: 'app-vehicle-form',
  templateUrl: './vehicle-form.component.html',
  styleUrls: ['./vehicle-form.component.css']
})
export class VehicleFormComponent implements OnInit {
    isNewMode: boolean = true;
    id: number;
    
    makes: any[] = [];
    models: any[] = [];
    vehicle: any = {};
    features: any[] = [];

    constructor(private route: ActivatedRoute, private vehicleService: VehicleService, private toastyService: ToastyService, private toastyConfig: ToastyConfig) {
        // possible values: default, bootstrap, material
        this.toastyConfig.theme = 'material';
    }

    ngOnInit() {
        this.route.params.subscribe(params => {
            this.id = +params['id'];
            if (this.id) {
                this.vehicleService.getVehicle(this.id).subscribe(vehicle => this.vehicle = vehicle);
                this.isNewMode = false;
            }
        })
        this.vehicleService.getMakes().subscribe(makes => this.makes = makes);
        this.vehicleService.getFeatures().subscribe(features => this.features = features);
        
    }

    onMakeChange() {
        var selectedMake = this.makes.find(m => m.id == this.vehicle.make);
        this.models = selectedMake ? selectedMake.models : [];
        console.log("VEHICLE", this.vehicle);
    }

    onSave() {
        //let vehicleResource: object = {
        //    modelId: this.vehicle.model.id,
        //    isRegistered: this.vehicle.isRegistered,
        //    contact: this.vehicle.contact,
        //    features: this.vehicle.Features.forEach(f => f.id),
        //}
    }

    onAdd() {

    }

    onUpdate() {
    
}

    onDelete() {
        if (confirm(`Are you sure you wish to remove car ${this.id}?`)) {
            this.vehicleService.deleteVehicle(this.id).subscribe(() => this.vehicle = {});
            this.addToast("Delete Success!", "Car was sucessfully delete.");
        }        
    }

    addToast(title: string, msg: string) {
      
        var toastOptions: ToastOptions = {
            title:'',
            msg:`<div class="alert alert-info"><strong> ${title} </strong> ${msg} </div>`,
            showClose: true,
            timeout: 5000,
            theme: 'bootstrap',
            onAdd: (toast: ToastData) => {
                console.log('Toast ' + toast.id + ' has been added!');
            },
            onRemove: function (toast: ToastData) {
                console.log('Toast ' + toast.id + ' has been removed!');
            }
        };

        // Add see all possible types in one shot
        //this.toastyService.info(toastOptions);
        this.toastyService.success(toastOptions);
        //this.toastyService.wait(toastOptions);
        //this.toastyService.error(toastOptions);
        //this.toastyService.warning(toastOptions);
    }

}

import { Pipe, PipeTransform } from '@angular/core';
import { Vehicle } from '../models/vehicle';
import { SortOptions } from '../models/globalenums';

@Pipe({
  name: 'sortVehicles'
})
export class SortVehiclesPipe implements PipeTransform {

    transform(value: Vehicle[], sortOption: SortOptions): Vehicle[] {
        switch (sortOption) {
            default:
            case SortOptions.none:
                return value;
            case SortOptions.make:
                return value.sort((a, b) => a.make.id - b.make.id);
            case SortOptions.model:
                return value.sort((a, b) => a.model.id - b.model.id);
                
            case SortOptions.contactName:
                return value.sort((a, b) => (+(a.contact.name > b.contact.name)) - (+(a.contact.name < b.contact.name)));
        }
  }

}

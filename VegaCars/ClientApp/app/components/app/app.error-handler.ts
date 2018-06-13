import { ErrorHandler, Inject } from "@angular/core";
import { ToastyService } from "ng2-toasty";



export class AppErrorHandler implements ErrorHandler {
    constructor(@Inject(ToastyService) private toastyService: ToastyService) { }

    handleError(error: any): void {
        // Global error handling code
        console.log("ERROR");

        this.toastyService.error({
            title: '',
            msg: "<div class='alert alert-danger'><strong> 'Error! <strong> An unexpected error happened.'</div>",
            theme: 'bootstrap',
            showClose: true,
            timeout: 5000
        });
    }
}
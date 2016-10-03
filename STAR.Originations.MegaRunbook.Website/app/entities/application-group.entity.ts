
import { ApplicationLink } from './application-link.entity';

export class ApplicationGroup {

    constructor(
        public Id:               number,
        public Name:             string,

        public ApplicationLinks: ApplicationLink[]
    ) { }

}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class Session {

    public Entity Entity { get; private set; }

    public Session(Entity entity) {
        if (entity.Type != EntityType.PC) {
            throw new ArgumentException("Cannot start session with non player entity");
        }

        this.Entity = entity;
    }

}
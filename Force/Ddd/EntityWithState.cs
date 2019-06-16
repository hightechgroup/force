namespace Force.Ddd
{
    public abstract class EntityWithState<TCode> 
    {
        public TCode StateCode { get; protected set; }

        private State<EntityWithState<TCode>> _state;
        
        protected abstract State<EntityWithState<TCode>> GetState(TCode stateCode);

        public State<EntityWithState<TCode>> State
            => _state ?? (_state = GetState(StateCode));
    }
}
using System.Collections.Generic;
using System.Linq;
using UnityEngine.Assertions;
using Random = UnityEngine.Random;

public class WeightedDistribution
{
    public int numWeights => _weights.Count;

    const float _fixedPointMultiplier = 1024.0f * 1024.0f;
    const float _minWeight = 1.0f / 1024.0f;
    const float _maxWeight = 10240.0f;

    readonly List<Weight> _weights = new();

    class Weight
    {
        public Weight(float frequency, int originalIndex)
        {
            averageTimeBetweenEvents = (int)(frequency * _fixedPointMultiplier + 0.5f);
            this.originalIndex = originalIndex;
            nextEventTime = averageTimeBetweenEvents;
        }

        public readonly int averageTimeBetweenEvents;
        public readonly int originalIndex;
        public int nextEventTime;
    }

    public WeightedDistribution(IEnumerable<float> il)
    {
        foreach (var w in il)
        {
            // since I'm using fixed point math, I only support a certain range
            Assert.IsTrue(w >= _minWeight);
            Assert.IsTrue(w <= _maxWeight);
            var index = _weights.Count;
            _weights.Add(new Weight(1.0f / w, index));
        }

        InitializeRandomness();
    }

    // you need to call this once after adding all the weights to this WeightedDistribution
    // otherwise the first couple of picks will always be deterministic.
    void InitializeRandomness()
    {
        foreach (var w in _weights)
        {
            w.nextEventTime = Random.Range(0, w.averageTimeBetweenEvents);
        }
    }

    // use this to pick a random item. it will give the distribution that you asked for
    // but try to not repeat the same item too often, or to let too much time pass
    // since an item was picked before it gets picked again.
    public int PickRandom()
    {
        var picked = _weights.OrderBy(w => w.nextEventTime).First();
        picked.nextEventTime += Random.Range(0, picked.averageTimeBetweenEvents);
        return picked.originalIndex;
    }
}